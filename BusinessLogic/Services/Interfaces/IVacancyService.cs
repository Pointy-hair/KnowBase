using BusinessLogic.DBContext;
using BusinessLogic.Helpers;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IVacancyService : IDisposable
    {
        Task<Vacancy> Get(int id);

        Task<ICollection<Vacancy>> Get(int skip, int amount);

        Task<Vacancy> Post(Vacancy vacancy, ICollection<int> candidates, int userId);

        Task<Vacancy> Update(Vacancy vacancy, ICollection<int> candidates, int userId);

        Task<ICollection<Vacancy>> Search(int skip, int amount, VacancySearchModel searchModel, VacancySortModel sortModel);

        Task<ICollection<Vacancy>> GetVacanciesById(ICollection<int> ids);

        Task<ICollection<Vacancy>> AutoVacanciesByCandidate(int candidateId, int coefficient, int skip, int amount);

        Task<Vacancy> UpdateStatus(int id, int status, int userId);

        Task<ReturnWrapper<Vacancy>> AssignCandidates(ICollection<int> vacancyIds, ICollection<int> candidateIds, int userId);

        Task<Vacancy> UnassignCandidate(int vacancyId, int candidateId, int userId);
    }
}
