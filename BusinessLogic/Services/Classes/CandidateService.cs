using BusinessLogic.DBContext;
using BusinessLogic.Unit_of_Work;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Tools;
using Microsoft.Practices.ObjectBuilder2;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Helpers;
using System.Diagnostics;

namespace BusinessLogic.Services.Classes
{
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ICandidateRepository candidateRepository;

        private readonly IContactRepository contactRepository;

        private readonly IPrevJobContactsRepository jobContactsRepository;

        private readonly IDictionaryRepository<CandidateStatus> statusRepository;

        private readonly IEventService eventService;

        private readonly IVacancyRepository vacancyRepository;

        public CandidateService(IUnitOfWork unitOfWork, 
            ICandidateRepository candidateRepository, 
            IContactRepository contactRepository, 
            IPrevJobContactsRepository jobContactsRepository,
            IDictionaryRepository<CandidateStatus> statusRepository, 
            IVacancyRepository vacancyRepository,
            IEventService eventService)
        {
            this.unitOfWork = unitOfWork;
            this.candidateRepository = candidateRepository;
            this.contactRepository = contactRepository;
            this.jobContactsRepository = jobContactsRepository;
            this.statusRepository = statusRepository;
            this.vacancyRepository = vacancyRepository;
            this.eventService = eventService;
        }

        public async Task<Candidate> GetById(int id)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            return candidate;
        }

        public async Task<ICollection<Candidate>> Find(int skip, int amount, CandidateSearchModel searchOptions, CandidateSortModel sortModel)
        {
            IEnumerable<Candidate> candidates;

            if (searchOptions != null)
            {
                candidates = await Task.Run(() => candidateRepository.Find(searchOptions));
            }
            else
            {
                candidates = await Task.Run(() => candidateRepository.ReadAll());
            }

            candidates = Sorting(candidates, sortModel);

            var result = candidates.Skip(skip).Take(amount).ToList();

            return result;
        }

        public async Task<ICollection<Candidate>> Paging(int skip, int amount, CandidateSortModel sortModel = null)
        {
            var result = await Task.Run(() => candidateRepository.PagingDeafaultOrderByDecending(skip, amount, s => s.Id));

            return result.ToList();
        }

        public async Task<Candidate> Update(Candidate candidate, ICollection<int> vacanciesIds)
        {
            return await Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    MappingVacansiesByIds(candidate, vacanciesIds);

                    MappingSkills(candidate.CandidateSecondarySkills);

                    MappingPrimarySkill(candidate.CandidatePrimarySkill);
                }).ConfigureAwait(false);

                var changedCandidate = await Task.Run(() => candidateRepository.Read(candidate.Id)).ConfigureAwait(false);

                changedCandidate.Contact = await Task.Run(() => ContactUpdate(changedCandidate.Contact, candidate.Contact)).ConfigureAwait(false);

                var result = await Task.Run(() => candidateRepository.Update(candidate)).ConfigureAwait(false);

                PrevJobUpdate(changedCandidate.CandidatePrevJobsContacts, candidate.CandidatePrevJobsContacts);

                return result;
            });
        }

        public async Task<Candidate> Add(Candidate candidate, ICollection<int> vacanciesIds)
        {
            return await Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    if (candidate.Status == 0)
                    {
                        candidate.Status = 1;
                    }

                    MappingVacansiesByIds(candidate, vacanciesIds);

                    MappingSkills(candidate.CandidateSecondarySkills);

                    MappingPrimarySkill(candidate.CandidatePrimarySkill);
                });

                var result = await Task.Run(() => candidateRepository.Create(candidate)).ConfigureAwait(false);

                return result;
            });
        }

        public async Task<ICollection<Candidate>> SetAttention(ICollection<Candidate> candidates)
        {
            var status = await Task.Run(() => statusRepository.Find(x => x.Name == "Attention").First());

            candidates.ForEach(c => c.CandidateStatus = status);

            return candidates;
        }

        public async Task<ICollection<Candidate>> AutoCandidatesByVacancy(int vacancyId, int skip, int take, int coeficPrimary)
        {
            var vacancy = await Task.Run(() => vacancyRepository.Read(vacancyId));

            var techSkill = vacancy.VacancyPrimarySkill?.TechSkill1;

            if (techSkill == null) return new List<Candidate>();

            var candidates =  techSkill.CandidatePrimarySkills.Select(x => x.Candidate1);

            var order = candidates.OrderBy(x => AutoSelectFunction.CompareByVacancy(vacancy, x, coeficPrimary));

            var result = order.Skip(skip).Take(take).ToList();

            return result;
        }

        public async Task<ICollection<Candidate>> GetCandidatesByIds(ICollection<int> ids)
        {
            var candidates = await Task.Run(() => candidateRepository.GetCandidatesByIds(ids));

            return candidates.ToList();
        }

        public async Task<ReturnWrapper<Candidate>> AssignVacancies(ICollection<int> vacIds, ICollection<int> canIds, int userId)
        {
            var candidatesToUpdate = await Task.Run(() => candidateRepository.GetCandidatesByIds(canIds));

            var vacanciesToAdd = await Task.Run(() => vacancyRepository.GetVacanciesById(vacIds));

            var events = new Dictionary<int, List<Event>>();

            foreach (var c in candidatesToUpdate)
            {
                c.LastModifier = userId;

                foreach (var v in vacanciesToAdd)
                {
                    v.LastModifier = userId;

                    if (!c.Vacancies.Contains(v))
                    {
                        c.Vacancies.Add(v);

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

            return new ReturnWrapper<Candidate>
            {
                Entities = candidatesToUpdate.ToList(),

                Events = events
            };
        }

        public async Task<Candidate> UnassignVacancy(int candidateId, int vacancyId, int userId)
        {
            var vacancy = await Task.Run(() => vacancyRepository.Read(vacancyId));

            var candidate = await Task.Run(() => candidateRepository.Read(candidateId));

            candidate.Vacancies.Remove(vacancy);

            candidate.Vacancies.ForEach(v => v.LastModifier = userId);

            candidate.LastModifier = userId;

            return candidate;
        }

        public async Task<Candidate> UpdateStatus(int id, int status, int userId)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            candidate.Status = status;

            candidate.LastModifier = userId;

            var result = await Task.Run(() => candidateRepository.Update(candidate));

            return result;
        }

        private static IEnumerable<Candidate> Sorting(IEnumerable<Candidate> candidates, CandidateSortModel sortModel)
        {
            if (sortModel == null)
            {
                candidates = candidates.OrderByDescending(x => x.Id);
                return candidates;
            }

            if (sortModel.CreationDate != null)
            {
                candidates = sortModel.RemindDate == true
                    ? candidates.OrderBy(x => x.Id)
                    : candidates.OrderByDescending(x => x.Id);
            }
            else if (sortModel.LastContactDate != null)
            {
                candidates = sortModel.RemindDate == true
                    ? candidates.OrderBy(x => x.LastContactDate)
                    : candidates.OrderByDescending(x => x.LastContactDate);
            }
            else if (sortModel.RemindDate != null)
            {
                candidates = sortModel.RemindDate == true
                    ? candidates.OrderBy(x => x.Reminder)
                    : candidates.OrderByDescending(x => x.Reminder);
            }
            else
            {
                candidates = candidates.OrderByDescending(x => x.Id);
            }

            return candidates;
        }

        private void MappingPrimarySkill(CandidatePrimarySkill skill)
        {
            if (skill == null)
            {
                return;
            }

            var techSkill = new TechSkill { Id = skill.TechSkill };

            unitOfWork.AttachToContext(techSkill);

            skill.TechSkill1 = techSkill;
        }

        private void MappingSkills(ICollection<CandidateSecondarySkill> candidateSkills)
        {
            foreach (var secSkill in candidateSkills)
            {
                var techSkill = new TechSkill { Id = secSkill.TechSkill };

                unitOfWork.AttachToContext(techSkill);

                secSkill.TechSkill1 = techSkill;
            }
        }

        private void MappingVacansiesByIds(Candidate candidate, ICollection<int> vacanciesIds)
        {
            if (vacanciesIds == null)
            {
                return;
            }

            foreach (var vacId in vacanciesIds)
            {
                var vacancy = new Vacancy { Id = vacId };

                unitOfWork.AttachToContext(vacancy);

                candidate.Vacancies.Add(vacancy);
            }
        }

        private void PrevJobUpdate(ICollection<CandidatePrevJobsContact> oldJobsContacts, 
            ICollection<CandidatePrevJobsContact> newJobsContacts)
        {
            var deleteJobsContacts = oldJobsContacts.Except(newJobsContacts, new PrevJobEqualityComparer()).ToList();

            var addJobsContacts = newJobsContacts.Except(oldJobsContacts, new PrevJobEqualityComparer()).ToList();

            deleteJobsContacts.ForEach(c =>
            {
                oldJobsContacts.Remove(c);

                jobContactsRepository.Delete(c.Id);
            });

            addJobsContacts.ForEach(oldJobsContacts.Add);

            foreach (var job in newJobsContacts)
            {
                if (job.Id == 0)
                {
                    continue;
                }

                var oldPrevJobs = oldJobsContacts.FirstOrDefault(x => x.Id == job.Id);

                ContactUpdate(oldPrevJobs.Contact, job.Contact);

                jobContactsRepository.Update(job);
            }
        }

        private Contact ContactUpdate(Contact oldContact, Contact changeContact)
        {
            oldContact = oldContact ?? changeContact;

            if (oldContact == null || oldContact == changeContact)
            {
                return oldContact;
            }

            if (changeContact != null)
            {
                changeContact.Id = oldContact.Id;

                contactRepository.Update(changeContact);
                return oldContact;
            }
            else
            {
                contactRepository.Delete(oldContact.Id);
                return null;
            }
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
