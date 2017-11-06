using BusinessLogic.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IMailService : IDisposable
    {
        Task<Mail> AddInterviewForCandidate(MailRequest mailRequest, int hrmId);

        Task<Mail> AddInterviewForInterviewer(MailRequest mailRequest, int hrmId);

        Task<Mail> ChangeInterviewDateForCandidate(MailRequest mailRequest, int hrmId);

        Task<Mail> ChangeInterviewDateForInterviewer(MailRequest mailRequest, int hrmId);

        Task<Mail> CancelInterviewForCandidate(MailRequest mailRequest, int hrmId);

        Task<Mail> CancelInterviewForInterviewer(MailRequest mailRequest, int hrmId);

        Task Send(Mail mail);
    }
}
