using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Models;
using BusinessLogic.Tools;
using BusinessLogic.Helpers;

namespace BusinessLogic.Services.Interfaces
{
    public interface ICandidateService : IDisposable
    {
        Task<ICollection<Candidate>> Paging(int skip, int amount, CandidateSortModel sortModel = null);

        Task<ICollection<Candidate>> Find(int skip, int amount, CandidateSearchModel searchOptions, CandidateSortModel sortModel);

        Task<Candidate> GetById(int id);

        Task<Candidate> Add(Candidate candidate, ICollection<int> vacanciesIds);

        Task<Candidate> Update(Candidate candidate, ICollection<int> vacanciesIds);

        Task<ICollection<Candidate>> SetAttention(ICollection<Candidate> candidates);

        Task<ICollection<Candidate>> AutoCandidatesByVacancy(int vacancyId, int skip, int take, int coeficPrimary);

        Task<ICollection<Candidate>> GetCandidatesByIds(ICollection<int> ids);

        Task<ReturnWrapper<Candidate>> AssignVacancies(ICollection<int> vacIds, ICollection<int> canIds, int userId);

        Task<Candidate> UpdateStatus(int id, int status, int userId);

        Task<Candidate> UnassignVacancy(int candidateId, int vacancyId, int userId);
    }
}
