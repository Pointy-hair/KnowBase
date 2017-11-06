using BusinessLogic.DBContext;
using BusinessLogic.Unit_of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Repository.UnityRepositories;

namespace BusinessLogic.Repository
{
    public class VacancyRepository : Repository<Vacancy>, IUpdateRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(IUnitOfWork context) : base(context)
        {
        }

        public IQueryable<Vacancy> Find(VacancySearchModel searchModel)
        {
            IQueryable<Vacancy> query = dbSet.AsNoTracking();

            if (searchModel.Status != 0)
            {
                query = query.Where(vac => vac.Status == searchModel.Status);
            }

            if (searchModel.City != 0)
            {
                query = query.Where(vac => vac.City == searchModel.City);
            }

            if (searchModel.PrimarySkill != null)
            {
                if (searchModel.PrimarySkill.Id != 0)
                {
                    query = query.Where(vac => vac.VacancyPrimarySkill.TechSkill == searchModel.PrimarySkill.Id);
                }
                if (searchModel.PrimarySkill.Level != 0)
                {
                    query = query.Where(vac => vac.VacancyPrimarySkill.Level == searchModel.PrimarySkill.Level);
                }
            }

            if (searchModel.ProjectName != null)
            {
                query = query.Where(vac => vac.ProjectName == searchModel.ProjectName);
            }

            if (searchModel.RequestDate != DateTime.MinValue)
            {
                query = query.Where(vac => vac.RequestDate == searchModel.RequestDate);
            }

            if (searchModel.StartDate != DateTime.MinValue)
            {
                query = query.Where(vac => vac.StartDate == searchModel.StartDate);
            }

            if (searchModel.VacancyName != null)
            {
                query = query.Where(vac => vac.VacancyName == searchModel.VacancyName);
            }
            return query;
        }

        public IEnumerable<Vacancy> GetVacanciesById(ICollection<int> ids)
        {
            var hashIds = new HashSet<int>(ids);

            var result = dbSet.Where(v => ids.Contains(v.Id));

            return result;
        }

        public Vacancy Update(Vacancy new_t)
        {
            var vacancy = dbSet.Find(new_t.Id);

            vacancy.City = new_t.City;

            vacancy.EngLevel = new_t.EngLevel;

            vacancy.Experience = new_t.Experience;

            vacancy.LastModifier = new_t.LastModifier;

            vacancy.Link = new_t.Link;

            vacancy.ProjectName = new_t.ProjectName;

            vacancy.RequestDate = new_t.RequestDate;

            vacancy.StartDate = new_t.StartDate;

            vacancy.CloseDate = new_t.CloseDate;

            vacancy.Status = new_t.Status;

            vacancy.VacancyName = new_t.VacancyName;

            vacancy.Candidates.Clear();

            if (new_t.Candidates != null)
            {
                foreach (var candidate in new_t.Candidates)
                {
                    vacancy.Candidates.Add(candidate);
                }
            }

            if (vacancy.VacancyPrimarySkill != null)
            {
                context.VacancyPrimarySkills.Remove(vacancy.VacancyPrimarySkill);
            }

            if (vacancy.VacancySecondarySkills != null)
            {
                var vacancySecondarySkillsToRemove = vacancy.VacancySecondarySkills.ToList();

                vacancy.VacancySecondarySkills.Clear();

                context.VacancySecondarySkills.RemoveRange(vacancySecondarySkillsToRemove);
            }

            if (new_t.VacancyPrimarySkill != null)
            {
                vacancy.VacancyPrimarySkill = new_t.VacancyPrimarySkill;
            }

            if (new_t.VacancySecondarySkills != null)
            {
                foreach (var vss in new_t.VacancySecondarySkills)
                {
                    vacancy.VacancySecondarySkills.Add(vss);
                }
            }

            return vacancy;
        }
    }
}
