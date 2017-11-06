using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Unit_of_Work;
using Moq;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.DBContext;

namespace BusinessLogic.Services.Classes.Tests
{
    [TestClass()]
    public class DictionaryServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;

        private Mock<IDictionaryRepository<City>> mockCityRepository;

        public DictionaryServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockCityRepository = new Mock<IDictionaryRepository<City>>();
        }

        [TestMethod()]
        public async Task GetAllTest()
        {
            City minsk = new City() { Name = "Minsk" };

            City vilnus = new City() { Name = "Vilnus" };

            City london = new City() { Name = "London" };

            City washington = new City() { Name = "Washington" };

            City vitebsk = new City() { Name = "Vitebsk" };

            IQueryable<City> cityList = new List<City>()
            {
                minsk,

                vilnus,

                london,

                washington,

                vitebsk
            }
            .AsQueryable();

            mockCityRepository.Setup(x => x.ReadAll()).Returns(cityList);

            var unitOfWork = mockUnitOfWork.Object;

            var cityRepository = mockCityRepository.Object;
            
            var dictionaryService = new DictionaryService<City>(unitOfWork, cityRepository);

            var result = await dictionaryService.GetAll();

            Assert.IsNotNull(result);

            Assert.AreEqual(cityList.Count(), result.Count);
        }
    }
}