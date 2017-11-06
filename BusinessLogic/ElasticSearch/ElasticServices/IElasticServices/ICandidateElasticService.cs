using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.Models;

namespace BusinessLogic.ElasticSearch.ElasticServices.IElasticServices
{
    public interface ICandidateElasticService
    {
        Task AddCandidateElastic(CandidateElasticModel elasticModel);

        Task<IEnumerable<CandidateElasticModel>> Find(int skip, int amount, CandidateSearchModel searchOptions,
            CandidateSortModel sortModel);

        Task UpdateCandidateElastic(CandidateElasticModel elasticModel);

        Task UpdateStatusElastic(int id, int status);///

        Task ManyElasticUpdate(IEnumerable<CandidateElasticModel> candidateElasticModels);

        Task SetAttention(ICollection<int> candidatesIds);///
    }
}
