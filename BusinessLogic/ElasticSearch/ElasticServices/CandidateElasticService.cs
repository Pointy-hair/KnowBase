using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticRepository.IRepository;
using BusinessLogic.ElasticSearch.ElasticServices.IElasticServices;
using BusinessLogic.Models;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.ElasticSearch.ElasticServices
{
    public class CandidateElasticService: ICandidateElasticService
    {
        private readonly ICandidateElasticRepository _candidateElasticRepository;
        private readonly IDictionaryRepository<CandidateStatus> _statusRepository;
        

        public CandidateElasticService(ICandidateElasticRepository candidateElasticRepository,
            IDictionaryRepository<CandidateStatus> statusRepository)
        {
            _statusRepository = statusRepository;
            _candidateElasticRepository = candidateElasticRepository;
        }



        public async Task<IEnumerable<CandidateElasticModel>> Find(int skip, int amount, 
            CandidateSearchModel searchOptions, CandidateSortModel sortModel)
        {
            var result = await Task.Run(() => _candidateElasticRepository.Search(skip, amount, searchOptions, sortModel));
            return result;
        }

        public Task AddCandidateElastic(CandidateElasticModel elasticModel)
        {
            return Task.Run(() => _candidateElasticRepository.AddCandidate(elasticModel));
        }

        public Task UpdateCandidateElastic(CandidateElasticModel elasticModel)
        {
            return Task.Run(() => _candidateElasticRepository.Update(elasticModel.Id, elasticModel));
        }

        public Task UpdateStatusElastic(int id, int status)
        {
            return Task.Run(() =>
            {
                var tehStatus = _statusRepository.Read(status);
                var candidate = _candidateElasticRepository.SearchById(id);

                candidate.Status = new StatusElasticModel { Id = status, Name = tehStatus.Name };
                _candidateElasticRepository.Update(id, candidate);
            });
        }

        public Task ManyElasticUpdate(IEnumerable<CandidateElasticModel> candidateElasticModels)
        {
            return Task.Run(() => _candidateElasticRepository.BulkInsertCandidates(candidateElasticModels));
        }

        public Task SetAttention(ICollection<int> candidatesIds)
        {
            return Task.Run(() =>
            {
                var attentionStatus = _statusRepository.Find(x => x.Name == "Attention").First();
                ICollection<CandidateElasticModel> candidates = new List<CandidateElasticModel>();
                foreach (var id in candidatesIds)
                {
                    var candidate = _candidateElasticRepository.SearchById(id);
                    candidate.Status.Name = attentionStatus.Name;
                    candidate.Status.Id = attentionStatus.Id;
                    candidates.Add(candidate);
                }
                _candidateElasticRepository.BulkInsertCandidates(candidates);
            });
        }
    }
}
