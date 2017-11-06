using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Models;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface ICandidateRepository : IDeleteRepository, ICreateRepository<Candidate>, 
        IReadRepository<Candidate>, IFindRepository<Candidate>, 
        IPagableRepository<Candidate>, IUpdateRepository<Candidate>
    {
        Candidate AssignVacancies(int id, ICollection<Vacancy> vacancies);
        IEnumerable<Candidate> GetCandidatesByIds(ICollection<int> ids);

        IQueryable<Candidate> Find(CandidateSearchModel filter);
    }
}
