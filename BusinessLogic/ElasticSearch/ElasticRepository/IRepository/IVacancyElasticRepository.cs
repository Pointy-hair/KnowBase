
using System.Collections.Generic;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.Models;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticRepository.IRepository
{
    public interface IVacancyElasticRepository
    {
        IBulkResponse BulkInsertCandidates(IEnumerable<VacancyElasticModel> candidates);
        IIndexResponse AddCandidate(VacancyElasticModel vacancy);

        IEnumerable<VacancyElasticModel> Search(int skip, int amount,
            VacancySearchModel searchModel, VacancySortModel sortModel);

        VacancyElasticModel SearchById(int id);

        IIndexResponse Update(int id, VacancyElasticModel model);
    }
}
