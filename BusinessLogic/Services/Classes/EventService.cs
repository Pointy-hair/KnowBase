using BusinessLogic.DBContext;
using BusinessLogic.Helpers;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Services.Tools;
using BusinessLogic.Tools;
using BusinessLogic.Unit_of_Work;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Classes
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;

        private readonly IVacancyRepository vacancyRepository;

        private readonly ICandidateRepository candidateRepository;

        private readonly ITechInterviewRepository techInterviewRepository;

        private readonly IGeneralInterviewRepository generalInterviewRepository;

        private readonly ObjectComparer comparer;

        private readonly ObjectStringifier stringifier;

        public EventService(ObjectComparer comparer, 
            ObjectStringifier stringifier,
            IEventRepository eventRepository,
            IVacancyRepository vacancyRepository,
            ICandidateRepository candidateRepository,
            ITechInterviewRepository techInterviewRepository,
            IGeneralInterviewRepository generalInterviewRepository)
        {
            this.comparer = comparer;

            this.stringifier = stringifier;

            this.eventRepository = eventRepository;

            this.vacancyRepository = vacancyRepository;

            this.candidateRepository = candidateRepository;

            this.techInterviewRepository = techInterviewRepository;

            this.generalInterviewRepository = generalInterviewRepository;
        }

        public async Task<ICollection<Event>> Paging(int skip, int amount)
        {
            return await Task.Run(() => eventRepository.PagingDeafaultOrderByDecending(skip, amount, c => c.Id).ToList());
        }

        public async Task<ICollection<Event>> Find(EventSearchOptions searchOptions)
        {
            var eventFilter = new EventFilter();

            eventRepository.Find(f => eventFilter.Filter(searchOptions, f));

            var result = await Task.Run(() => eventRepository.PagingInFound(0, 10, x => x.Id));

            return result.ToList();
        }

        public async Task<Event> RegisterCandidate(Candidate candidate, int userId)
        {
            var newEvent = new Event
            {
                Candidate1 = candidate,

                Date = DateTime.UtcNow,

                Event1 = EventIds.Candidate,

                EventType = EventTypes.Add,

                User = userId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterVacancy(Vacancy vacancy, int userId)
        {
            var newEvent = new Event
            {
                Date = DateTime.UtcNow,

                Event1 = EventIds.Vacancy,

                EventType = EventTypes.Add,

                User = userId,

                Vacancy1 = vacancy
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterGeneralInterview(GeneralInterview interview, int userId)
        {
            var newEvent = new Event
            {
                Date = DateTime.UtcNow,

                Event1 = EventIds.Interview,

                EventType = EventTypes.Add,

                GeneralInterview1 = interview,

                User = userId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterTechInterview(TechInterview interview, int userId)
        {
            var newEvent = new Event
            {
                Date = DateTime.UtcNow,

                Event1 = EventIds.Interview,

                EventType = EventTypes.Add,

                TechInterview1 = interview,

                User = userId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterCandidateToVacancyAssignment(int vacancyId, int candidateId, int userId)
        {
            var newEvent = new Event
            {
                Candidate = candidateId,

                Date = DateTime.UtcNow,

                Event1 = EventIds.Vacancy,

                EventType = EventTypes.Edit,

                Property = "candidates add",

                User = userId,

                Vacancy = vacancyId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterCandidateFromVacancyUnassignment(int vacancyId, int candidateId, int userId)
        {
            var newEvent = new Event
            {
                Candidate = candidateId,

                Date = DateTime.UtcNow,

                Event1 = EventIds.Vacancy,

                EventType = EventTypes.Edit,

                Property = "candidates remove",

                User = userId,

                Vacancy = vacancyId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterCandidateStatusUpdate(int candidateId, int newStatus, int userId)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(candidateId));

            var oldStatus = candidate.Status;

            var newEvent = new Event
            {
                Candidate = candidateId,

                Date = DateTime.UtcNow,

                Event1 = EventIds.Candidate,

                EventType = EventTypes.Edit,

                NewValue = newStatus.ToString(),

                OldValue = oldStatus.ToString(),

                Property = "status",

                User = userId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<Event> RegisterVacancyStatusUpdate(int vacancyId, int newStatus, int userId)
        {
            var vacancy = await Task.Run(() => vacancyRepository.Read(vacancyId));

            var oldStatus = vacancy.Status;

            var newEvent = new Event
            {
                Date = DateTime.UtcNow,

                Event1 = EventIds.Vacancy,

                EventType = EventTypes.Edit,

                NewValue = newStatus.ToString(),

                OldValue = oldStatus.ToString(),

                Property = "status",

                User = userId,

                Vacancy = vacancyId
            };

            var result = await Task.Run(() => eventRepository.Create(newEvent));

            return result;
        }

        public async Task<ICollection<Event>> RegisterCandidateUpdate(Candidate newCandidate, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var oldCandidate = await Task.Run(() => candidateRepository.Read(newCandidate.Id));

                    var eventsList = new List<Event>();

                    if (!comparer.IsEqual(oldCandidate.CandidatePrimarySkill, newCandidate.CandidatePrimarySkill))
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = stringifier.GetStr(newCandidate.CandidatePrimarySkill),

                            OldValue = stringifier.GetStr(oldCandidate.CandidatePrimarySkill),

                            Property = "primary skill",

                            User = userId
                        });
                    }

                    foreach (var oldSkill in oldCandidate.CandidateSecondarySkills)
                    {
                        var changed = true;

                        foreach (var newSkill in newCandidate.CandidateSecondarySkills)
                        {
                            if (comparer.IsEqual(oldSkill, newSkill))
                            {
                                changed = false;

                                break;
                            }
                        }

                        if (changed)
                        {
                            eventsList.Add(new Event
                            {
                                Candidate = oldCandidate.Id,

                                Date = DateTime.UtcNow,

                                Event1 = EventIds.Candidate,

                                EventType = EventTypes.Edit,

                                OldValue = stringifier.GetStr(oldSkill),

                                Property = "secondary skill",

                                User = userId
                            });
                        }
                    }

                    foreach (var newSkill in newCandidate.CandidateSecondarySkills)
                    {
                        var added = true;

                        foreach (var oldSkill in oldCandidate.CandidateSecondarySkills)
                        {
                            if (comparer.IsEqual(oldSkill, newSkill))
                            {
                                added = false;

                                break;
                            }
                        }

                        if (added)
                        {
                            eventsList.Add(new Event
                            {
                                Candidate = oldCandidate.Id,

                                Date = DateTime.UtcNow,

                                Event1 = EventIds.Candidate,

                                EventType = EventTypes.Edit,

                                NewValue = stringifier.GetStr(newSkill),

                                Property = "secondary skill",

                                User = userId
                            });
                        }
                    }

                    if (oldCandidate.City != newCandidate.City)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.City.HasValue ? newCandidate.City.Value.ToString() : null,

                            OldValue = oldCandidate.City.HasValue ? oldCandidate.City.Value.ToString() : null,

                            Property = "city",

                            User = userId
                        });
                    }

                    if (!comparer.IsEqual(oldCandidate.Contact, newCandidate.Contact))
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = stringifier.GetStr(newCandidate.Contact),

                            OldValue = stringifier.GetStr(oldCandidate.Contact),

                            Property = "contacts",

                            User = userId
                        });
                    }

                    if (oldCandidate.CustomerInterviewDate != newCandidate.CustomerInterviewDate)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.CustomerInterviewDate.HasValue ? newCandidate.CustomerInterviewDate.Value.ToString() : null,

                            OldValue = oldCandidate.CustomerInterviewDate.HasValue ? oldCandidate.CustomerInterviewDate.Value.ToString() : null,

                            Property = "customer interview date",

                            User = userId
                        });
                    }

                    if (oldCandidate.CustomerInterviewStatus != newCandidate.CustomerInterviewStatus)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.CustomerInterviewStatus.HasValue ? newCandidate.CustomerInterviewStatus.Value.ToString() : null,

                            OldValue = oldCandidate.CustomerInterviewStatus.HasValue ? oldCandidate.CustomerInterviewStatus.Value.ToString() : null,

                            Property = "customer interview status",

                            User = userId
                        });
                    }

                    if (oldCandidate.DesiredSalary != newCandidate.DesiredSalary)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.DesiredSalary.HasValue ? newCandidate.DesiredSalary.Value.ToString() : null,

                            OldValue = oldCandidate.DesiredSalary.HasValue ? oldCandidate.DesiredSalary.Value.ToString() : null,

                            Property = "desired salary",

                            User = userId
                        });
                    }

                    if (oldCandidate.EngLevel != newCandidate.EngLevel)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.EngLevel.HasValue ? newCandidate.EngLevel.Value.ToString() : null,

                            OldValue = oldCandidate.EngLevel.HasValue ? oldCandidate.EngLevel.Value.ToString() : null,

                            Property = "english level",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.FirstNameEng, newCandidate.FirstNameEng, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.FirstNameEng,

                            OldValue = oldCandidate.FirstNameEng,

                            Property = "first name english",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.FirstNameRus, newCandidate.FirstNameRus, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.FirstNameRus,

                            OldValue = oldCandidate.FirstNameRus,

                            Property = "first name russiasn",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.LastNameEng, newCandidate.LastNameEng, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.LastNameEng,

                            OldValue = oldCandidate.LastNameEng,

                            Property = "last name english",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.LastNameRus, newCandidate.LastNameRus, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.LastNameRus,

                            OldValue = oldCandidate.LastNameRus,

                            Property = "last name russsian",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.Picture, newCandidate.Picture, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.Picture,

                            OldValue = oldCandidate.Picture,

                            Property = "picture",

                            User = userId
                        });
                    }

                    if (oldCandidate.PSExperience != newCandidate.PSExperience)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.PSExperience.HasValue ? newCandidate.PSExperience.Value.ToString() : null,

                            OldValue = oldCandidate.PSExperience.HasValue ? oldCandidate.PSExperience.Value.ToString() : null,

                            Property = "primary skill experience",

                            User = userId
                        });
                    }

                    if (string.Compare(oldCandidate.Resume, newCandidate.Resume, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.Resume,

                            OldValue = oldCandidate.Resume,

                            Property = "resume",

                            User = userId
                        });
                    }

                    if (oldCandidate.Status != newCandidate.Status)
                    {
                        eventsList.Add(new Event
                        {
                            Candidate = oldCandidate.Id,

                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Candidate,

                            EventType = EventTypes.Edit,

                            NewValue = newCandidate.Status.ToString(),

                            OldValue = oldCandidate.Status.ToString(),

                            Property = "status",

                            User = userId
                        });
                    }

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
            
        }

        public async Task<ICollection<Event>> RegisterVacancyUpdate(Vacancy newVacancy, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var oldVacancy = await Task.Run(() => vacancyRepository.Read(newVacancy.Id));

                    var eventsList = new List<Event>();

                    if (oldVacancy.City != newVacancy.City)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.City.HasValue ? newVacancy.City.Value.ToString() : null,

                            OldValue = oldVacancy.City.HasValue ? oldVacancy.City.Value.ToString() : null,

                            Property = "city",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.CloseDate != newVacancy.CloseDate)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.CloseDate.HasValue ? newVacancy.CloseDate.Value.ToString() : null,

                            OldValue = oldVacancy.CloseDate.HasValue ? oldVacancy.CloseDate.Value.ToString() : null,

                            Property = "closure date",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.EngLevel != newVacancy.EngLevel)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.EngLevel.HasValue ? newVacancy.EngLevel.Value.ToString() : null,

                            OldValue = oldVacancy.EngLevel.HasValue ? oldVacancy.EngLevel.Value.ToString() : null,

                            Property = "english level",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.Experience != newVacancy.Experience)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.Experience.HasValue ? newVacancy.Experience.Value.ToString() : null,

                            OldValue = oldVacancy.Experience.HasValue ? oldVacancy.Experience.Value.ToString() : null,

                            Property = "experience",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (string.Compare(oldVacancy.Link, newVacancy.Link, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.Link,

                            OldValue = oldVacancy.Link,

                            Property = "link",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (string.Compare(oldVacancy.ProjectName, newVacancy.ProjectName, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.ProjectName,

                            OldValue = oldVacancy.ProjectName,

                            Property = "project name",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (string.Compare(oldVacancy.VacancyName, newVacancy.VacancyName, StringComparison.Ordinal) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.VacancyName,

                            OldValue = oldVacancy.VacancyName,

                            Property = "vacancy name",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.RequestDate != newVacancy.RequestDate)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.RequestDate.HasValue ? newVacancy.RequestDate.Value.ToString() : null,

                            OldValue = oldVacancy.RequestDate.HasValue ? oldVacancy.RequestDate.Value.ToString() : null,

                            Property = "request date",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.StartDate != newVacancy.StartDate)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.StartDate.HasValue ? newVacancy.StartDate.Value.ToString() : null,

                            OldValue = oldVacancy.StartDate.HasValue ? oldVacancy.StartDate.Value.ToString() : null,

                            Property = "start date",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (oldVacancy.Status != newVacancy.Status)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = newVacancy.Status.ToString(),

                            OldValue = oldVacancy.Status.ToString(),

                            Property = "status",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    if (!comparer.IsEqual(oldVacancy.VacancyPrimarySkill, newVacancy.VacancyPrimarySkill))
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Vacancy,

                            EventType = EventTypes.Edit,

                            NewValue = stringifier.GetStr(newVacancy.VacancyPrimarySkill),

                            OldValue = stringifier.GetStr(oldVacancy.VacancyPrimarySkill),

                            Property = "primary skill",

                            User = userId,

                            Vacancy = oldVacancy.Id
                        });
                    }

                    foreach (var oldSkill in oldVacancy.VacancySecondarySkills)
                    {
                        var changed = true;

                        foreach (var newSkill in newVacancy.VacancySecondarySkills)
                        {
                            if (comparer.IsEqual(oldSkill, newSkill))
                            {
                                changed = false;

                                break;
                            }
                        }

                        if (changed)
                        {
                            eventsList.Add(new Event
                            {
                                Date = DateTime.UtcNow,

                                Event1 = EventIds.Vacancy,

                                EventType = EventTypes.Edit,

                                OldValue = stringifier.GetStr(oldSkill),

                                Property = "secondary skill",

                                User = userId,

                                Vacancy = oldVacancy.Id
                            });
                        }
                    }

                    foreach (var newSkill in newVacancy.VacancySecondarySkills)
                    {
                        var added = true;

                        foreach (var oldSkill in oldVacancy.VacancySecondarySkills)
                        {
                            if (comparer.IsEqual(oldSkill, newSkill))
                            {
                                added = false;

                                break;
                            }
                        }

                        if (added)
                        {
                            eventsList.Add(new Event
                            {
                                Date = DateTime.UtcNow,

                                Event1 = EventIds.Vacancy,

                                EventType = EventTypes.Edit,

                                NewValue = stringifier.GetStr(newSkill),

                                Property = "secondary skill",

                                User = userId,

                                Vacancy = oldVacancy.Id
                            });
                        }
                    }

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
        }

        public async Task<ICollection<Event>> RegisterGeneralInterviewUpdate(GeneralInterview newInterview, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var oldInterview = await Task.Run(() => generalInterviewRepository.Read(newInterview.Id));

                    var eventsList = new List<Event>();

                    if (oldInterview.City != newInterview.City)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.City.HasValue ? newInterview.City.Value.ToString() : null,

                            OldValue = oldInterview.City.HasValue ? oldInterview.City.Value.ToString() : null,

                            Property = "city",

                            User = userId
                        });
                    }

                    if (string.Compare(oldInterview.Commentary, newInterview.Commentary, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Commentary,

                            OldValue = oldInterview.Commentary,

                            Property = "commentary",

                            User = userId
                        });
                    }

                    if (oldInterview.Date != newInterview.Date)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Date.HasValue ? newInterview.Date.Value.ToString() : null,

                            OldValue = oldInterview.Date.HasValue ? oldInterview.Date.Value.ToString() : null,

                            Property = "date",

                            User = userId
                        });
                    }

                    if (oldInterview.Interviewer != newInterview.Interviewer)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Interviewer.HasValue ? newInterview.Interviewer.Value.ToString() : null,

                            OldValue = oldInterview.Interviewer.HasValue ? oldInterview.Interviewer.Value.ToString() : null,

                            Property = "interviewer",

                            User = userId
                        });
                    }

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
        }

        public async Task<ICollection<Event>> RegisterTechInterviewUpdate(TechInterview newInterview, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var oldInterview = await Task.Run(() => techInterviewRepository.Read(newInterview.Id));

                    var eventsList = new List<Event>();

                    if (oldInterview.City != newInterview.City)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.City.HasValue ? newInterview.City.Value.ToString() : null,

                            OldValue = oldInterview.City.HasValue ? oldInterview.City.Value.ToString() : null,

                            Property = "city",

                            User = userId
                        });
                    }

                    if (string.Compare(oldInterview.Commentary, newInterview.Commentary, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Commentary,

                            OldValue = oldInterview.Commentary,

                            Property = "commentary",

                            User = userId
                        });
                    }

                    if (oldInterview.Date != newInterview.Date)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Date.HasValue ? newInterview.Date.Value.ToString() : null,

                            OldValue = oldInterview.Date.HasValue ? oldInterview.Date.Value.ToString() : null,

                            Property = "date",

                            User = userId
                        });
                    }

                    if (oldInterview.Interviewer != newInterview.Interviewer)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.Interviewer.HasValue ? newInterview.Interviewer.Value.ToString() : null,

                            OldValue = oldInterview.Interviewer.HasValue ? oldInterview.Interviewer.Value.ToString() : null,

                            Property = "interviewer",

                            User = userId
                        });
                    }

                    if (oldInterview.TechSkill != newInterview.TechSkill)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Interview,

                            EventType = EventTypes.Edit,

                            GeneralInterview = oldInterview.Id,

                            NewValue = newInterview.TechSkill.ToString(),

                            OldValue = oldInterview.TechSkill.ToString(),

                            Property = "skill",

                            User = userId
                        });
                    }

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
        }

        public async Task<ICollection<Event>> RegisterGeneralInterviewFeedback(GeneralInterview newInterview, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var interviewId = newInterview.Id;

                    var eventsList = new List<Event>();

                    if (newInterview.EngLevel != null)
                    {
                        eventsList.Add(new Event
                        {
                            Date = DateTime.UtcNow,

                            Event1 = EventIds.Feedback,

                            EventType = EventTypes.Add,

                            GeneralInterview = interviewId,

                            NewValue = newInterview.EngLevel.ToString(),

                            Property = "english level",

                            User = userId
                        });
                    }

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        GeneralInterview = interviewId,

                        NewValue = newInterview.Feedback,

                        Property = "feedback",

                        User = userId
                    });

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        GeneralInterview = interviewId,

                        NewValue = newInterview.Interests,

                        Property = "interests",

                        User = userId
                    });

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        GeneralInterview = interviewId,

                        NewValue = newInterview.JobChangeReason,

                        Property = "job change reason",

                        User = userId
                    });

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        GeneralInterview = interviewId,

                        NewValue = newInterview.ReadyForBusinessTrips.HasValue ? newInterview.ReadyForBusinessTrips.Value.ToString() : null,

                        Property = "ready for business trips",

                        User = userId
                    });

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        GeneralInterview = interviewId,

                        NewValue = newInterview.WhenCanStart.HasValue ? newInterview.WhenCanStart.Value.ToString() : null,

                        Property = "when can start",

                        User = userId
                    });

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
        }

        public async Task<ICollection<Event>> RegisterTechInterviewFeedback(TechInterview newInterview, int userId)
        {
            return await Task.Run
            (
                async () =>
                {
                    var interviewId = newInterview.Id;

                    var eventsList = new List<Event>();

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        TechInterview = interviewId,

                        NewValue = newInterview.Mark.HasValue ? newInterview.Mark.Value.ToString() : null,

                        Property = "mark",

                        User = userId
                    });

                    eventsList.Add(new Event
                    {
                        Date = DateTime.UtcNow,

                        Event1 = EventIds.Feedback,

                        EventType = EventTypes.Add,

                        TechInterview = interviewId,

                        NewValue = newInterview.Feedback,

                        Property = "interests",

                        User = userId
                    });

                    var result = await Task.Run(() => eventRepository.AddRange(eventsList));

                    return result.ToList();
                }
            );
        }
    }
}
