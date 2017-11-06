using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Tools;

namespace BusinessLogic.Services.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<Event>> Paging(int skip, int amount);

        Task<ICollection<Event>> Find(EventSearchOptions searchOptions);

        Task<Event> RegisterCandidate(Candidate candidate, int userId);

        Task<Event> RegisterVacancy(Vacancy vacancy, int userId);

        Task<Event> RegisterGeneralInterview(GeneralInterview interview, int userId);

        Task<Event> RegisterTechInterview(TechInterview interview, int userId);

        Task<Event> RegisterCandidateToVacancyAssignment(int vacancyId, int candidateId, int userId);

        Task<Event> RegisterCandidateFromVacancyUnassignment(int vacancyId, int candidateId, int userId);

        Task<Event> RegisterVacancyStatusUpdate(int vacancyId, int newStatus, int userId);

        Task<Event> RegisterCandidateStatusUpdate(int candidateId, int newStatus, int userId);

        Task<ICollection<Event>> RegisterCandidateUpdate(Candidate newCandidate, int userId);

        Task<ICollection<Event>> RegisterVacancyUpdate(Vacancy newVacancy, int userId);

        Task<ICollection<Event>> RegisterGeneralInterviewUpdate(GeneralInterview newInterview, int userId);

        Task<ICollection<Event>> RegisterTechInterviewUpdate(TechInterview newInterview, int userId);

        Task<ICollection<Event>> RegisterGeneralInterviewFeedback(GeneralInterview newInterview, int userId);

        Task<ICollection<Event>> RegisterTechInterviewFeedback(TechInterview newInterview, int userId);
    }
}
