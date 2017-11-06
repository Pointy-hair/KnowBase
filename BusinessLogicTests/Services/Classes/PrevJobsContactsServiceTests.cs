using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using Moq;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;
using System.Collections.Generic;

namespace BusinessLogic.Services.Classes.Tests
{
    [TestClass()]
    public class PrevJobsContactsServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;

        private Mock<IPrevJobContactsRepository> mockPrevJobContactsRepository;

        private Mock<ICandidateRepository> mockCandidateRepository;

        public PrevJobsContactsServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockPrevJobContactsRepository = new Mock<IPrevJobContactsRepository>();

            mockCandidateRepository = new Mock<ICandidateRepository>();
        }

        [TestMethod()]
        public async Task GetByCandidateIdTest()
        {
            int candidateId = 9;

            var candidate = new Candidate()
            {
                Id = 9,
                CandidatePrevJobsContacts = new List<CandidatePrevJobsContact>()
                {
                    new CandidatePrevJobsContact()
                    {
                        Id = 8,
                        CompanyName = "Exadel",
                        Position = "JavaScript Developer",
                        Contact = new Contact()
                            {
                            Phone = "+375332781754",
                            Email = "email@gmail.com"
                            }
                    },
                    new CandidatePrevJobsContact()
                    {
                        Id = 2,
                        CompanyName = "EPAM",
                        Position = "Java Junior Developer",
                        Contact = new Contact()
                            {
                            Email = "dffgh@gmail.com"
                            }
                    }
                }
            };

            mockCandidateRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(candidate);

            var candidateRepository = mockCandidateRepository.Object;

            var unitOfWork = mockUnitOfWork.Object;

            var jobContactsRepository = mockPrevJobContactsRepository.Object;

            var prevJobsContactsService = new PrevJobsContactsService(unitOfWork, jobContactsRepository, candidateRepository);

            var result = await prevJobsContactsService.GetByCandidateId(candidateId);

            Assert.IsNotNull(result);
        }
    }
}