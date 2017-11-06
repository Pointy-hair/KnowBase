using BusinessLogic.DBContext;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Classes.Tests
{
    [TestClass()]
    public class InterviewServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;

        private Mock<ITechInterviewRepository> mockTechRepository;

        private Mock<ICandidateRepository> mockCandidateRepository;

        private Mock<IGeneralInterviewRepository> mockGeneralRepository;

        private Mock<IDictionaryRepository<CandidateStatus>> mockStatusRepository;

        public InterviewServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockTechRepository = new Mock<ITechInterviewRepository>();

            mockCandidateRepository = new Mock<ICandidateRepository>();

            mockGeneralRepository = new Mock<IGeneralInterviewRepository>();

            mockStatusRepository = new Mock<IDictionaryRepository<CandidateStatus>>();
        }

        [TestMethod()]
        public async Task AddTechInterviewTest()
        {
            TechInterview techInterview = new TechInterview()
            {
                Id = 3,

                Candidate = 4,

                City = 1,

                Date = new DateTime(2016,5,16),

                Status = true,

                TechSkill = 5,

                Commentary = "Commentary"
            };

            Candidate candidate = new Candidate()
            {
                Id = 5,

                FirstNameEng = "Andrey",

                LastNameEng = "Popov",

                FirstNameRus = "Андрей",

                LastNameRus = "Попов",

                Status = 5,

                HRM = 2
            };

            CandidateStatus candidateStatus = new CandidateStatus(){ Name = "Pool" };

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate);

            IQueryable<CandidateStatus> candidateStatusList = new List<CandidateStatus>() { candidateStatus }.AsQueryable();

            mockStatusRepository.Setup(x => x.Find(It.IsAny<Expression<Func<CandidateStatus, bool>>>())).Returns(candidateStatusList);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var techRepository = mockTechRepository.Object;

            var generalRepository = mockGeneralRepository.Object;

            var statusRepository = mockStatusRepository.Object;

            var interviewService = new InterviewService(unitOfWork, techRepository, candidateRepository,
                generalRepository, statusRepository);

            await interviewService.AddTechInterview(techInterview);

            Assert.IsTrue(candidate.TechInterviewStatus.Value);
        }

        [TestMethod()]
        public async Task AddGeneralInterviewTest()
        {
            GeneralInterview generalInterview = new GeneralInterview()
            {
                Id = 5,

                Candidate = 2,

                City = 3,

                Date = new DateTime(2017, 7, 21),

                Status = false,

                EngLevel = 4,

                Commentary = "Commentary"
            };

            Candidate candidate = new Candidate()
            {
                Id = 7,

                FirstNameEng = "Aleksey",

                LastNameEng = "Mironov",

                FirstNameRus = "Алексей",

                LastNameRus = "Миронов",

                Status = 3,

                HRM = 1
            };

            CandidateStatus candidateStatus = new CandidateStatus() { Name = "On hold" };

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate);

            IQueryable<CandidateStatus> candidateStatusList = new List<CandidateStatus>() { candidateStatus }.AsQueryable();

            mockStatusRepository.Setup(x => x.Find(It.IsAny<Expression<Func<CandidateStatus, bool>>>())).Returns(candidateStatusList);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var techRepository = mockTechRepository.Object;

            var generalRepository = mockGeneralRepository.Object;

            var statusRepository = mockStatusRepository.Object;

            var interviewService = new InterviewService(unitOfWork, techRepository, candidateRepository,
                generalRepository, statusRepository);

            await interviewService.AddGeneralInterview(generalInterview);

            Assert.IsTrue(candidate.GeneralInterviewStatus.Value);
        }
    }
}