using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using Moq;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Classes.Tests
{
    [TestClass()]
    public class VacancyServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;

        private Mock<IVacancyRepository> mockVacancyRepository;

        private Mock<ICandidateRepository> mockCandidateRepository;

        private Mock<IDictionaryRepository<TechSkill>> mockTechSkillRepository;

        private Mock<ICandidateService> mockCandidateService;

        private Mock<IEventService> mockEventService;

        public VacancyServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockVacancyRepository = new Mock<IVacancyRepository>();

            mockCandidateRepository = new Mock<ICandidateRepository>();

            mockTechSkillRepository = new Mock<IDictionaryRepository<TechSkill>>();

            mockCandidateService = new Mock<ICandidateService>();

            mockEventService = new Mock<IEventService>();
        }

        [TestMethod()]
        public async Task UpdateVacancyStatusTest()
        {
            int status = 4;

            var vacancy = new Vacancy()
            {
                Id = 18,

                Status = 1,

                HRM = 2
            };

            var newVacancy = new Vacancy()
            {
                Id = 18,

                Status = 4,

                HRM = 2
            };

            mockVacancyRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(vacancy);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var candidateService = mockCandidateService.Object;

            var techSkillRepository = mockTechSkillRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var vacancyService = new VacancyService(unitOfWork, vacancyRepository, candidateRepository, techSkillRepository,
                candidateService, eventService);

            var result = await vacancyService.UpdateStatus(vacancy.Id, status, vacancy.HRM);

            Assert.AreEqual(newVacancy.Status, result.Status);
        }

        [TestMethod()]
        public async Task GetTest()
        {
            int vacancyId = 15;

            var vacancy = new Vacancy()
            {
                Id = 15,

                VacancyName = "iOS Developer"
            };

            mockVacancyRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(vacancy);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var candidateService = mockCandidateService.Object;

            var techSkillRepository = mockTechSkillRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var vacancyService = new VacancyService(unitOfWork, vacancyRepository, candidateRepository, techSkillRepository,
                candidateService, eventService);

            var result = await vacancyService.Get(vacancyId);
            
            Assert.AreEqual(vacancy, result);
        }

        [TestMethod()]
        public async Task SearchTest()
        {
            Vacancy vacancy1 = new Vacancy()
            {
                Id = 22,

                Status = 1,

                ProjectName = "Some project name",

                City = 4
            };

            Vacancy vacancy2 = new Vacancy()
            {
                Id = 2,

                Status = 4,

                ProjectName = "Another project name",

                City = 2
            };

            VacancySearchModel searchModel = new VacancySearchModel()
            {
                VacancyName = "JavaScript Junior Developer",

                City = 3,

                Status = 2,

                RequestDate = new DateTime(2017,8,8)
            };

            IQueryable<Vacancy> vacancyFindList = new List<Vacancy>().AsQueryable();

            IQueryable<Vacancy> vacancyReadList = new List<Vacancy>() { vacancy1, vacancy2 }.AsQueryable();

            mockVacancyRepository.Setup(x => x.Find(It.IsAny<VacancySearchModel>())).Returns(vacancyFindList);

            mockVacancyRepository.Setup(x => x.ReadAll()).Returns(vacancyReadList);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var candidateService = mockCandidateService.Object;

            var techSkillRepository = mockTechSkillRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var vacancyService = new VacancyService(unitOfWork, vacancyRepository, candidateRepository, techSkillRepository,
                candidateService, eventService);
            
            var result = await vacancyService.Search(0, 5, searchModel, null);

            Assert.AreEqual(0, result.Count);

            var actual = await vacancyService.Search(0, 5, null, null);

            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod()]
        public async Task UnassignCandidateTest()
        {
            var vacancy = new Vacancy()
            {
                Id = 18,

                Status = 1,

                HRM = 2
            };

            Candidate candidate1 = new Candidate()
            {
                Id = 22,

                FirstNameEng = "David",

                LastNameEng = "Fraller",

                FirstNameRus = "Давид",

                LastNameRus = "Фраллер",

                Status = 1
            };

            Candidate candidate2 = new Candidate()
            {
                Id = 2,

                FirstNameEng = "Victor",

                LastNameEng = "Toll",

                FirstNameRus = "Виктор",

                LastNameRus = "Толл",

                Status = 3
            };

            vacancy.Candidates.Add(candidate1);

            vacancy.Candidates.Add(candidate2);

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate1);

            mockVacancyRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(vacancy);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var candidateService = mockCandidateService.Object;

            var techSkillRepository = mockTechSkillRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var vacancyService = new VacancyService(unitOfWork, vacancyRepository, candidateRepository, techSkillRepository,
                candidateService, eventService);

            var result = await vacancyService.UnassignCandidate(vacancy.Id, candidate1.Id, vacancy.HRM);

            Assert.AreEqual(1, result.Candidates.Count);
        }
    }
}