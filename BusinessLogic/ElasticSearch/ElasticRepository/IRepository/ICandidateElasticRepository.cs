using System.Collections.Generic;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.Models;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticRepository.IRepository
{
    public interface ICandidateElasticRepository
    {
        IBulkResponse BulkInsertCandidates(IEnumerable<CandidateElasticModel> candidates);

        IIndexResponse AddCandidate(CandidateElasticModel candidate);

        IEnumerable<CandidateElasticModel> Search(int skip, int amount,
            CandidateSearchModel searchModel, CandidateSortModel sortModel);

        CandidateElasticModel SearchById(int id);

        IIndexResponse Update(int id, CandidateElasticModel model);
    }
}
