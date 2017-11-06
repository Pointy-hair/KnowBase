using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using BusinessLogic.DBContext;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Tools;
using BusinessLogic.Unit_of_Work;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using BusinessLogic.Repository.UnityRepositories;

namespace BusinessLogic.Services.Classes
{
    public class FileService : IFileService
    {
        private const string ResumeContainerName = "resumecontainer";

        private const string ExcelContainerName = "excelcontainer";

        private readonly CloudStorageAccount storageAccount;

        private readonly CloudBlobClient blobClient;

        private readonly IUnitOfWork unitOfWork;

        private readonly ICandidateRepository candidateRepository;

        private readonly ICandidateService candidateService;

        private readonly IVacancyService vacancyService;

        public FileService(IUnitOfWork unitOfWork, 
            ICandidateRepository candidateRepository, 
            ICandidateService candidateService, 
            IVacancyService vacancyService)
        {
            this.storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("KnowBaseFSContext"));

            this.blobClient = storageAccount.CreateCloudBlobClient();

            this.unitOfWork = unitOfWork;
            this.candidateRepository = candidateRepository;
            this.candidateService = candidateService;
            this.vacancyService = vacancyService;
        }


        public void AddStringToFile(string containerName, string fileName, string content)
        {
            var container = OpenOrInitializeContainer(containerName);

            CloudAppendBlob appendBlob = container.GetAppendBlobReference(fileName);

            //Create the append blob. Note that if the blob already exists, the CreateOrReplace() method will overwrite it.
            //You can check whether the blob exists to avoid overwriting it by using CloudAppendBlob.Exists().
            appendBlob.CreateOrReplace();

            appendBlob.AppendText($"Timestamp: {DateTime.UtcNow} \tLog Entry: {content}");

            appendBlob.DownloadText();
        }

        public async Task<HttpContent> GetResumeContentById(int id)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            var fileName = candidate.Resume;

            var container = OpenOrInitializeContainer(ResumeContainerName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            var stream = new MemoryStream();

            blockBlob.DownloadToStream(stream);

            var content = CreateContent(stream, fileName);


            return content;
        }

        public string AddFile(string containerName, string newFileName, HttpFileCollection files)
        {
            if (files.Count <= 0)
            {
                throw new Exception("File not found");
            }

            var postedFile = files[0];

            var fileName = postedFile.FileName;

            var typeFile = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));

            var container = OpenOrInitializeContainer(containerName);

            var blockBlob = container.GetBlockBlobReference($"{newFileName}{typeFile}");

            using (var stream = postedFile.InputStream)
            {
                blockBlob.UploadFromStream(stream);
            }
            return typeFile;
        }

        public async Task AddResumeById(int id, HttpFileCollection files)
        {
            var typeFile = AddFile(ResumeContainerName, $"Resume{id}", files);

            var candidate = await Task.Run(() => candidateRepository.Read(id));

            candidate.Resume = $"Resume{id}{typeFile}";

            unitOfWork.Save();
        }

        public async Task<HttpContent> GetExcelByCandidates(int skip, int amount, 
            CandidateSearchModel searchModel, CandidateSortModel sortModel)
        {
            var stream = new MemoryStream(await AddToExcelCandidate(skip, amount, searchModel, sortModel));

            var content = CreateContent(stream, "Candidates.xlsx");

            return content;
        }

        public async Task<HttpContent> GetExcelByVacancies(int skip, int amount, 
            VacancySearchModel searchModel, VacancySortModel sortModel)
        {
            var stream = new MemoryStream(await AddToExcelVacancy(skip, amount, searchModel, sortModel));

            var result = CreateContent(stream, "Vacancies.xlsx");

            return result;
        }

        public async Task<byte[]> AddToExcelCandidate(int skip, int amount,
            CandidateSearchModel searchOptions, CandidateSortModel sortModel)
        {
            var candidates = await candidateService.Find(skip, amount, searchOptions, sortModel);

            var excelCandidateList = candidates.Select(ConvertToExcelModel).ToList();

            var result = ExcelCreator.GenerateExcel(excelCandidateList);

            return result;
        }

        public async Task<byte[]> AddToExcelVacancy(int skip, int amount, 
            VacancySearchModel searchModel, VacancySortModel sortModel)
        {
            var vacancies = await vacancyService.Search(skip, amount, searchModel, sortModel);

            var excelVacancyList = vacancies.Select(ConvertToExcelModel).ToList();

            var result = ExcelCreator.GenerateExcel(excelVacancyList);

            return result;
        }


        private CloudBlobContainer OpenOrInitializeContainer(string containerName)
        {
            var container = blobClient.GetContainerReference(containerName);
          //  SetPublicContainerPermissions(container);
            container.CreateIfNotExists();
            return container;
        }

        private static HttpContent CreateContent(Stream stream, string fileName)
        {
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            content.Headers.Add("content-disposition", $"attachment; filename={fileName}");
            content.Headers.ContentLength = stream.Length;
            return content;
        }

        private static ExcelCandidateModel ConvertToExcelModel(Candidate candidate)
        {
            var candidateModel = new ExcelCandidateModel
            {
                FirstNameEng = candidate.FirstNameEng,
                LastNameEng = candidate.LastNameEng,
                FirstNameRus = candidate.FirstNameRus,
                LastNameRus = candidate.LastNameRus,
                Status = candidate.CandidateStatus?.Name,
                PrimarySkill = candidate.CandidatePrimarySkill?.TechSkill1?.Name,
                PrimarySkillLevel = candidate.CandidatePrimarySkill?.Level ?? 0,
                HRM = candidate.User?.Name,
                Phone = candidate.Contact?.Phone,
                Email = candidate.Contact?.Email,
                Skype = candidate.Contact?.Skype,
                LinkedIn = candidate.Contact?.LinkedIn,
                EngLevel = candidate.EngLevel1?.Name,
                City = candidate.City1?.Name,
                PSExperience = candidate.PSExperience ?? DateTime.MinValue,
                DesiredSalary = candidate.DesiredSalary ?? 0,
                LastContactDate = candidate.LastContactDate ?? DateTime.MinValue
            };
            
            string allSecondarySkills = candidate.CandidateSecondarySkills
                .Aggregate("", (current, skill) => current + skill.TechSkill1.Name + "(" + skill.Level + "), ");

            if (allSecondarySkills != "")
            {
                allSecondarySkills = allSecondarySkills.Remove(allSecondarySkills.Length - 2);
            }
            candidateModel.SecondarySkills = allSecondarySkills;
            return candidateModel;
        }

        private static ExcelVacancyModel ConvertToExcelModel(Vacancy vacancy)
        {
            ExcelVacancyModel vacancyModel = new ExcelVacancyModel()
            {
                ProjectName = vacancy.ProjectName,
                VacancyName = vacancy.VacancyName,
                Status = vacancy.VacancyStatus?.Name,
                Link = vacancy.Link,
                HRM = vacancy.User?.Name,
                StartDate = vacancy.StartDate ?? DateTime.MinValue,
                EngLevel = vacancy.EngLevel1?.Name,
                Experience = vacancy.Experience,
                City = vacancy.City1?.Name,
                PrimarySkill = vacancy.VacancyPrimarySkill?.TechSkill1?.Name,
                PrimarySkillLevel = vacancy.VacancyPrimarySkill?.Level ?? 0,
                CloseDate = vacancy.CloseDate ?? DateTime.MinValue
            };

            string allSecondarySkills = "";

            foreach (var skill in vacancy.VacancySecondarySkills)
            {
                allSecondarySkills += skill.TechSkill1.Name + "(" + skill.Level + "), ";
            }

            if (allSecondarySkills != "")
            {
                allSecondarySkills = allSecondarySkills.Remove(allSecondarySkills.Length - 2);
            }

            vacancyModel.SecondarySkills = allSecondarySkills;

            return vacancyModel;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
