using BusinessLogic.DBContext;
using BusinessLogic.Services.Tools;
using BusinessLogic.Unit_of_Work;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Repository.UnityRepositories;
using Microsoft.Practices.ObjectBuilder2;
using BusinessLogic.Helpers;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using System.Diagnostics;

namespace BusinessLogic.Services.Classes
{
    public class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IVacancyRepository vacancyRepository;

        private readonly ICandidateRepository candidateRepository;

        private readonly IDictionaryRepository<TechSkill> techSkillRepository;

        private readonly ICandidateService candidateService;

        private readonly IEventService eventService;

        public VacancyService(IUnitOfWork unitOfWork, 
            IVacancyRepository vacancyRepository, 
            ICandidateRepository candidateRepository, 
            IDictionaryRepository<TechSkill> techSkillRepository,
            ICandidateService candidateService,
            IEventService eventService)
        {
            this.unitOfWork = unitOfWork;

            this.vacancyRepository = vacancyRepository;

            this.candidateRepository = candidateRepository;

            this.techSkillRepository = techSkillRepository;

            this.candidateService = candidateService;

            this.eventService = eventService;
        }

        public async Task<Vacancy> Get(int id)
        {
            var res = await Task.Run(() => vacancyRepository.Read(id))
                .ConfigureAwait(false);

            return res;
        }

        public async Task<ICollection<Vacancy>> Get(int skip, int amount)
        {
            var res = await Task.Run(() => vacancyRepository.PagingDefaultOrder(skip, amount, v => v.Id).ToList());

            return res;
        }

        public Task<Vacancy> Post(Vacancy vacancy, ICollection<int> candidates, int userId)
        {
            return Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    AttachNewCandidates(vacancy, candidates);

                    SetUpNulls(vacancy);

                    vacancy.HRM = userId;

                    vacancy.LastModifier = userId;
                }).ConfigureAwait(false);

                return await Task.Run(() => vacancyRepository.Create(vacancy)).ConfigureAwait(false);
            });
        }

        public Task<Vacancy> Update(Vacancy vacancy, ICollection<int> candidates, int userId)
        {
            return Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    vacancy.LastModifier = userId;

                    AttachNewCandidates(vacancy, candidates);

                    SetUpNulls(vacancy);
                }).ConfigureAwait(false);

                await ApplyStatusChange(vacancy.Id, vacancy.Status);

                return await Task.Run(() => vacancyRepository.Update(vacancy)).ConfigureAwait(false);
            });
        }

        public async Task<ICollection<Vacancy>> Search(int skip, int amount, VacancySearchModel searchModel, VacancySortModel sortModel)
        {
            IQueryable<Vacancy> vacancies;

            vacancies = searchModel == null ? vacancyRepository.ReadAll() : vacancyRepository.Find(searchModel);


            var res = Sorting(vacancies, sortModel).Skip(skip).Take(amount);

            var vacancyList = await Task.Run(() => res.ToList());

            return vacancyList;
        }

        public async Task<ICollection<Vacancy>> GetVacanciesById(ICollection<int> ids)
        {
            var result = await Task.Run(() => vacancyRepository.GetVacanciesById(ids).ToList());

            return result;
        }

        public async Task<ICollection<Vacancy>> AutoVacanciesByCandidate(int candidateId, int coefficient, int skip, int amount)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(candidateId));
            
            if(candidate.CandidatePrimarySkill == null) return new List<Vacancy>();

            var techSkills = await Task.Run(() => techSkillRepository.Read(candidate.CandidatePrimarySkill.TechSkill));

            var vacancies = new List<Vacancy>();

            techSkills.VacancyPrimarySkills.ForEach(prSkills => vacancies.Add(prSkills.Vacancy1));

            //foreach (var v in vacancies)
            //{
            //    Debug.WriteLine(v.Id + " " + AutoSelectFunction.CompareByCandidate(candidate, v, coefficient));
            //}

            var ordered = vacancies.OrderBy(x => AutoSelectFunction.CompareByCandidate(candidate, x, coefficient));

            //foreach (var v in ordered)
            //{
            //    Debug.WriteLine(v.Id);
            //}

            return ordered.Skip(skip).Take(amount).ToList();
        }

        public async Task<Vacancy> UpdateStatus(int id, int status, int userId)
        {
            if (id == 0 || status == 0)
            {
                return null;
            }

            var vacancy = await ApplyStatusChange(id, status);

            vacancy.Status = status;

            vacancy.LastModifier = userId;

            return vacancy;
        }

        public async Task<ReturnWrapper<Vacancy>> AssignCandidates(ICollection<int> vacancyIds, ICollection<int> candidateIds, int userId)
        {
            var vacanciesToUpdate = await Task.Run(() => vacancyRepository.GetVacanciesById(vacancyIds));

            var candidatesToAdd = await Task.Run(() => candidateRepository.GetCandidatesByIds(candidateIds.ToList()));

            var events = new Dictionary<int, List<Event>>();

            foreach (var v in vacanciesToUpdate)
            {
                v.LastModifier = userId;

                foreach (var c in candidatesToAdd)
                {
                    c.LastModifier = userId;

                    if (!v.Candidates.Contains(c))
                    {
                        v.Candidates.Add(c);

                        var e = await eventService.RegisterCandidateToVacancyAssignment(v.Id, c.Id, userId);

                        var eventsOfUser = new List<Event>();

                        if (events.TryGetValue(v.HRM, out eventsOfUser))
                        {
                            eventsOfUser.Add(e);
                        }
                        else
                        {
                            events.Add(v.HRM, new List<Event> { e });
                        }

                        if (events.TryGetValue(c.HRM, out eventsOfUser))
                        {
                            eventsOfUser.Add(e);
                        }
                        else
                        {
                            events.Add(c.HRM, new List<Event> { e });
                        }
                    }
                }
            }

            return new ReturnWrapper<Vacancy>
            {
                Entities = vacanciesToUpdate.ToList(),

                Events = events
            };
        }

        public async Task<Vacancy> UnassignCandidate(int vacancyId, int candidateId, int userId)
        {
            var vacancy = await Task.Run(() => vacancyRepository.Read(vacancyId));

            var candidate = await Task.Run(() => candidateRepository.Read(candidateId));

            vacancy.Candidates.Remove(candidate);

            vacancy.LastModifier = userId;

            vacancy.Candidates.ForEach(c => c.LastModifier = userId);

            return vacancy;
        }

        private void AttachNewSkills(Vacancy v, TechSkillModel primarySkill, ICollection<TechSkillModel> secondarySkills)
        {
            if (primarySkill != null)
            {
                var pTechSkill = new TechSkill { Id = primarySkill.Id };

                unitOfWork.AttachToContext<TechSkill>(pTechSkill);

                v.VacancyPrimarySkill = new VacancyPrimarySkill
                {
                    TechSkill1 = pTechSkill,

                    Level = primarySkill.Level
                };
            }

            if (secondarySkills != null)
            {
                foreach (var skill in secondarySkills)
                {
                    var sTechSkill = new TechSkill { Id = skill.Id };

                    unitOfWork.AttachToContext<TechSkill>(sTechSkill);

                    v.VacancySecondarySkills.Add(new VacancySecondarySkill
                    {
                        TechSkill1 = sTechSkill,

                        Level = skill.Level
                    });
                }
            }
        }

        private void AttachNewCandidates(Vacancy v, ICollection<int> candidates)
        {
            if (candidates != null)
            {
                foreach (var candidate in candidates)
                {
                    var exisitingCandidate = new Candidate { Id = candidate };

                    unitOfWork.AttachToContext<Candidate>(exisitingCandidate);

                    v.Candidates.Add(exisitingCandidate);
                }
            }
        }

        private void SetUpNulls(Vacancy v)
        {
            if (v.City.Value == 0)
            {
                v.City = null;
            }

            if (v.EngLevel.Value == 0)
            {
                v.EngLevel = null;
            }
        }

        private async Task<Vacancy> ApplyStatusChange(int vacancyId, int status)
        {
            var existingVacancy = await Task.Run(() => vacancyRepository.Read(vacancyId));

            if (existingVacancy.Status != VacancyStatuses.Cancelled && existingVacancy.Status != VacancyStatuses.Closed)
            {
                if (status == VacancyStatuses.Closed || status == VacancyStatuses.Cancelled)
                {
                    await candidateService.SetAttention(existingVacancy.Candidates.ToList());
                }
            }

            return existingVacancy;
        }

        private static IEnumerable<Vacancy> Sorting(IEnumerable<Vacancy> vacancies, VacancySortModel sortModel)
        {
            if (sortModel == null)
            {
               
                vacancies = vacancies.OrderByDescending(x => x.Id);
                return vacancies;
                
            }

            if (sortModel.StartDate != null)
            {
                vacancies = sortModel.StartDate == true
                    ? vacancies.OrderBy(x => x.StartDate)
                    : vacancies.OrderByDescending(x => x.StartDate);
            }
            else if (sortModel.RequestDate != null)
            {
                vacancies = sortModel.RequestDate == true
                    ? vacancies.OrderBy(x => x.RequestDate)
                    : vacancies.OrderByDescending(x => x.RequestDate);
            }
            else if (sortModel.CloseDate != null)
            {
                vacancies = sortModel.CloseDate == true
                    ? vacancies.OrderBy(x => x.CloseDate)
                    : vacancies.OrderByDescending(x => x.CloseDate);
            }
            else
            {
                vacancies = vacancies.OrderByDescending(x => x.Id);
            }

            return vacancies;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
