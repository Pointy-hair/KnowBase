
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.Models;

namespace BusinessLogic.ElasticSearch.ElasticServices.IElasticServices
{
    public interface IVacancyElasticService
    {
        Task<IEnumerable<VacancyElasticModel>> Search(int skip, int amount,
            VacancySearchModel searchModel, VacancySortModel sortModel);

        Task AddVacancyElastic(VacancyElasticModel elasticModel);

        Task UpdateStatusElastic(int id, int status);

        Task UpdateVacancyElastic(VacancyElasticModel elasticModel);
    }
}
