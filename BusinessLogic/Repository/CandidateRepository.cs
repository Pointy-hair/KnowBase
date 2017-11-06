using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Unit_of_Work;
using Microsoft.Practices.ObjectBuilder2;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Models;

namespace BusinessLogic.Repository
{
    public class CandidateRepository : Repository<Candidate>, IUpdateRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(IUnitOfWork context) : base(context)
        {
        }

        public IQueryable<Candidate> Find(CandidateSearchModel filter)
        {
            IQueryable<Candidate> query = dbSet.AsNoTracking();
            if (filter.Email != null)
            {
                query = query.Where(candidate => candidate.Contact != null && candidate.Contact.Email == filter.Email);
            }

            if (filter.Phone != null)
            {
                query = query.Where(candidate => candidate.Contact != null &&
                                                 candidate.Contact.Phone == filter.Phone);
            }

            if (filter.Skype != null)
            {
                query = query.Where(candidate => candidate.Contact != null &&
                                                 candidate.Contact.Skype == filter.Skype);
            }

            if (filter.HRM != null)
            {
                query = query.Where(candidate => candidate.HRM == filter.HRM);
            }

            if (filter.LastNameEng != null)
            {
                query = query.Where(candidate => candidate.LastNameEng == filter.LastNameEng);
            }

            if (filter.LastNameRus != null)
            {
                query = query.Where(candidate => candidate.LastNameRus == filter.LastNameRus);
            }

            if (filter.PrimarySkill != null)
            {
                query = query.Where(candidate => candidate.CandidatePrimarySkill != null &&
                                                 candidate.CandidatePrimarySkill.TechSkill == filter.PrimarySkill);
            }

            if (filter.Level != null)
            {
                query = query.Where(candidate => candidate.CandidatePrimarySkill != null &&
                                                 candidate.CandidatePrimarySkill.Level == filter.Level);
            }

            if (filter.City != null)
            {
                query = query.Where(candidate => candidate.City == filter.City);
            }

            if (filter.Status != null)
            {
                query = query.Where(candidate => candidate.Status == filter.Status);
            }

            return query;
        }

        public Candidate Update(Candidate updated)
        {
            Candidate cn = dbSet.Find(updated.Id);
            cn.FirstNameEng = updated.FirstNameEng;
            cn.LastNameEng = updated.LastNameEng;
            cn.FirstNameRus = updated.FirstNameRus;
            cn.LastNameRus = updated.LastNameRus;
            cn.City = updated.City;
            cn.PSExperience = updated.PSExperience;
            cn.DesiredSalary = updated.DesiredSalary;
            cn.EngLevel = updated.EngLevel;
            cn.LastContactDate = updated.LastContactDate;
            cn.Status = updated.Status;
            cn.Reminder = updated.Reminder;
            cn.Picture = updated.Picture;
            cn.LastModifier = updated.LastModifier;
            cn.CustomerInterviewDate = updated.CustomerInterviewDate;
            cn.CustomerInterviewEndDate = updated.CustomerInterviewEndDate;
            if(updated.TechInterviewStatus != null)
            {
                cn.TechInterviewStatus = updated.TechInterviewStatus;
            }
            if (updated.GeneralInterviewStatus != null)
            {
                cn.GeneralInterviewStatus = updated.GeneralInterviewStatus;
            }
            if (updated.CustomerInterviewStatus != null)
            {
                cn.CustomerInterviewStatus = updated.CustomerInterviewStatus;
            }
            if (updated.CandidatePrimarySkill != null)
            {
                cn.CandidatePrimarySkill = cn.CandidatePrimarySkill ?? new CandidatePrimarySkill();
                cn.CandidatePrimarySkill.Level = updated.CandidatePrimarySkill.Level;
                cn.CandidatePrimarySkill.TechSkill = updated.CandidatePrimarySkill.TechSkill;
            }
            else
            {
                cn.CandidatePrimarySkill = null;
            }
            cn.CandidateSecondarySkills.Clear();
            updated.CandidateSecondarySkills.ForEach(c => cn.CandidateSecondarySkills.Add(c));


            IEnumerable<Vacancy> deleteVacancies = cn.Vacancies.Except(updated.Vacancies).ToList();
            IEnumerable<Vacancy> addVacancies = updated.Vacancies.Except(cn.Vacancies).ToList();
            deleteVacancies.ForEach(c => cn.Vacancies.Remove(c));
            addVacancies.ForEach(c => cn.Vacancies.Add(c));

            return cn;
        }

        public Candidate AssignVacancies(int id, ICollection<Vacancy> vacancies)
        {
            Candidate candidate = dbSet.Find(id);
            vacancies.ForEach(x => candidate.Vacancies.Add(x));
            return candidate;
        }

        public IEnumerable<Candidate> GetCandidatesByIds(ICollection<int> ids)
        {
            HashSet<int> idsSet = new HashSet<int>(ids);
            var queryable = dbSet.Where(x => idsSet.Contains(x.Id));
            return queryable;
        }
    }

}
