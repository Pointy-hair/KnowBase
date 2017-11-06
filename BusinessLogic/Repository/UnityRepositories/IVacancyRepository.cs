using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Models;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface IVacancyRepository : IReadRepository<Vacancy>, IDeleteRepository, 
        ICreateRepository<Vacancy>, IFindRepository<Vacancy>, IUpdateRepository<Vacancy>, IPagableRepository<Vacancy>
    {
        IEnumerable<Vacancy> GetVacanciesById(ICollection<int> ids);

        IQueryable<Vacancy> Find(VacancySearchModel searchModel);
    }
}
