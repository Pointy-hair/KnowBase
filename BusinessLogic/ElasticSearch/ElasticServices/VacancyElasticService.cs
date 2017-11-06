using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticRepository;
using BusinessLogic.ElasticSearch.ElasticRepository.IRepository;
using BusinessLogic.ElasticSearch.ElasticServices.IElasticServices;
using BusinessLogic.Models;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;

namespace BusinessLogic.ElasticSearch.ElasticServices
{
    public class VacancyElasticService: IVacancyElasticService
    {
        private readonly IVacancyElasticRepository _vacancyElasticRepository;

        private readonly IDictionaryRepository<CandidateStatus> _statusRepository;

        private readonly IVacancyRepository vacancyRepository;

        public VacancyElasticService(IVacancyElasticRepository vacancyElasticRepository,
            IDictionaryRepository<CandidateStatus> statusRepository,
            IVacancyRepository vacancyRepository)
        {
            this.vacancyRepository = vacancyRepository;
            _statusRepository = statusRepository;
            _vacancyElasticRepository = vacancyElasticRepository;
        }

        public async Task<IEnumerable<VacancyElasticModel>> Search(int skip, int amount,
            VacancySearchModel searchModel, VacancySortModel sortModel)
        {
            var result = await Task.Run(() => _vacancyElasticRepository.Search(skip, amount, searchModel, sortModel));
            return result;
        }

        public Task AddVacancyElastic(VacancyElasticModel elasticModel)
        {
            return Task.Run(() => _vacancyElasticRepository.AddCandidate(elasticModel));
        }

        public Task UpdateStatusElastic(int id, int status)
        {
            return Task.Run(() =>
            {
                var tehStatus = _statusRepository.Read(status);
                var vacancy = _vacancyElasticRepository.SearchById(id);
                vacancy.Status = new StatusElasticModel { Id = status, Name = tehStatus.Name };
                _vacancyElasticRepository.Update(id, vacancy);
            });
        }

        public Task UpdateVacancyElastic(VacancyElasticModel elasticModel)
        {
            return Task.Run(() => _vacancyElasticRepository.Update(elasticModel.Id, elasticModel));
        }
    }
}
