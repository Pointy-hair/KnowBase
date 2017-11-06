using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using BusinessLogic.Unit_of_Work;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.DBContext;
using Moq;
using System.Collections.Generic;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Models;
using System.Linq;

namespace BusinessLogic.Services.Classes.Tests
{
    [TestClass()]
    public class CandidateServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;

        private Mock<ICandidateRepository> mockCandidateRepository;

        private Mock<IContactRepository> mockContactRepository;

        private Mock<IPrevJobContactsRepository> mockJobContactsRepository;

        private Mock<IDictionaryRepository<CandidateStatus>> mockStatusRepository;

        private Mock<IVacancyRepository> mockVacancyRepository;

        private Mock<IEventService> mockEventService;

        public CandidateServiceTests()
        {
            mockCandidateRepository = new Mock<ICandidateRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockContactRepository = new Mock<IContactRepository>();
            mockJobContactsRepository = new Mock<IPrevJobContactsRepository>();
            mockStatusRepository = new Mock<IDictionaryRepository<CandidateStatus>>();
            mockVacancyRepository = new Mock<IVacancyRepository>();
            mockEventService = new Mock<IEventService>();
        }

        [TestMethod()]
        public async Task GetByIdTest()
        {
            int candidateId = 901;

            var candidate = new Candidate()
            {
                Id = 901,

                FirstNameEng = "Fred",

                Status = 4
            };

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var contactRepository = mockContactRepository.Object;

            var jobContactsRepository = mockJobContactsRepository.Object;

            var statusRepository = mockStatusRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var candidateService = new CandidateService(unitOfWork, candidateRepository, contactRepository,
                jobContactsRepository, statusRepository, vacancyRepository, eventService);

            var result = await candidateService.GetById(candidateId);
            
            Assert.AreEqual(candidate, result);
        }
        
        [TestMethod()]
        public async Task UpdateCandidateStatusTest()
        {
            int status = 3;

            var candidate = new Candidate()
            {
                Id = 18,

                Status = 6,

                HRM = 2
            };

            var newCandidate = new Candidate()
            {
                Id = 18,

                Status = 3,

                HRM = 2
            };

            VacancyPrimarySkill vacancyPrimSkill = new VacancyPrimarySkill();

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate);

            mockCandidateRepository.Setup(x => x.Update(It.IsAny<Candidate>())).Returns(candidate);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var contactRepository = mockContactRepository.Object;

            var jobContactsRepository = mockJobContactsRepository.Object;

            var statusRepository = mockStatusRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var candidateService = new CandidateService(unitOfWork, candidateRepository, contactRepository,
                jobContactsRepository, statusRepository, vacancyRepository, eventService);

            var result = await candidateService.UpdateStatus(candidate.Id, status, candidate.HRM);

            Assert.AreEqual(newCandidate.Status, result.Status);
        }

        [TestMethod()]
        public async Task FindTest()
        {
            Candidate candidate1 = new Candidate()
            {
                Id = 22,

                FirstNameEng = "David",

                LastNameEng = "Fraller",

                FirstNameRus = "Давид",

                LastNameRus = "Фраллер",

                Status = 1,

                City = 2
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
            
            CandidateSearchModel searchModel = new CandidateSearchModel()
            {
                LastNameEng = "David",

                City = 2,

                Status = 1
            };

            IQueryable<Candidate> candidateFindList = new List<Candidate>(){ candidate1 }.AsQueryable();

            IQueryable<Candidate> candidateReadList = new List<Candidate>() { candidate1, candidate2 }.AsQueryable();

            mockCandidateRepository.Setup(x => x.Find(It.IsAny<CandidateSearchModel>())).Returns(candidateFindList);

            mockCandidateRepository.Setup(x => x.ReadAll()).Returns(candidateReadList);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var contactRepository = mockContactRepository.Object;

            var jobContactsRepository = mockJobContactsRepository.Object;

            var statusRepository = mockStatusRepository.Object;

            var vacancyRepository = mockVacancyRepository.Object;

            var eventService = mockEventService.Object;

            var candidateService = new CandidateService(unitOfWork, candidateRepository, contactRepository,
                jobContactsRepository, statusRepository, vacancyRepository, eventService);

            var result = await candidateService.Find(0,5,searchModel,null);

            Assert.IsNotNull(result);

            var actual = await candidateService.Find(0,5,null,null);

            Assert.AreEqual(2, actual.Count);
        }
    }
}