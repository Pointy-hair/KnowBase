using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Services.Classes
{
    public class PrevJobsContactsService : IPrevJobsContactsService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IPrevJobContactsRepository prevJobContactsRepository;

        private readonly ICandidateRepository candidateRepository;

        public PrevJobsContactsService(IUnitOfWork unitOfWork, 
            IPrevJobContactsRepository prevJobContactsRepository, 
            ICandidateRepository candidateRepository)
        {
            this.unitOfWork = unitOfWork;

            this.prevJobContactsRepository = prevJobContactsRepository;

            this.candidateRepository = candidateRepository;
        }

        public async Task<List<CandidatePrevJobsContact>> GetByCandidateId(int id)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            return candidate.CandidatePrevJobsContacts.ToList();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
