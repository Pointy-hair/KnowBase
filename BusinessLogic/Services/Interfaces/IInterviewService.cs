using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;

namespace BusinessLogic.Services.Interfaces
{
    public interface IInterviewService : IDisposable
    {
        Task<ICollection<TechInterview>> GetTechInterviewsByCandidate(int id);

        Task<ICollection<GeneralInterview>> GetGeneralInterviewsByCandidate(int id);

        Task<TechInterview> AddTechInterview(TechInterview techInterview);

        Task<TechInterview> UpdateTechInterview(TechInterview techInterview);

        Task<GeneralInterview> AddGeneralInterview(GeneralInterview generalInterview);

        Task<GeneralInterview> UpdateGeneralInterview(GeneralInterview generalInterview);

        Task<GeneralInterview> SetGeneralInterviewFeedback(GeneralInterview generalInterview);

        Task<TechInterview> SetTechInterviewFeedback(TechInterview techInterview);

        Task AddCustomerInterview(int candidateId, DateTime date, DateTime endDate);

        Task UpdateCustomerInterview(int candidateId, DateTime? date, DateTime? endDate);

        Task CustomerInterviewClose(int candidateId);
    }
}
