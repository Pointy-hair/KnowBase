using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Classes
{
    public class MailService : IMailService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserRepository userRepository;

        private readonly ICandidateRepository candidateRepository;

        private readonly string senderEmail = "knowbaseinfo@gmail.com";

        private readonly string senderName = "Exadel";

        private readonly string senderPassword = "mypasswordforexadel";

        public MailService(IUnitOfWork unitOfWork, 
            IUserRepository userRepository,
            ICandidateRepository candidateRepository)
        {
            this.unitOfWork = unitOfWork;

            this.userRepository = userRepository;

            this.candidateRepository = candidateRepository;
        }

        public async Task<Mail> AddInterviewForCandidate(MailRequest mailRequest, int hrmId )
        {
            var candidate = await Task.Run(()=>candidateRepository.Read(mailRequest.Candidate));

            var hrm =await Task.Run(()=>userRepository.Read(hrmId));

            var mail = AddFormInterviewForCandidate(hrm, candidate, mailRequest.Skill, mailRequest.Date);

            return mail;
        }

        public async Task<Mail> AddInterviewForInterviewer(MailRequest mailRequest, int hrmId)
        {
            
            var candidate = await Task.Run(() =>candidateRepository.Read(mailRequest.Candidate));

            var hrm = await Task.Run(()=>userRepository.Read(hrmId));

            var interviewer = await Task.Run(()=>userRepository.Read(mailRequest.Interviewer));

            var mail = AddFormInterviewForInterviewer(hrm, interviewer, mailRequest.Skill,
                mailRequest.Date, candidate);

            return mail;
        }

        public async Task<Mail> ChangeInterviewDateForCandidate(MailRequest mailRequest, int hrmId)
        {
            var candidate = await Task.Run(()=>candidateRepository.Read(mailRequest.Candidate));

            var hrm = await Task.Run(()=>userRepository.Read(hrmId));

            var mail = ChangeFormInterviewDateForCandidate(hrm, candidate, mailRequest.Date);

            return mail;
        }

        public async Task<Mail> ChangeInterviewDateForInterviewer(MailRequest mailRequest, int hrmId)
        {
            var candidate = await Task.Run(()=>candidateRepository.Read(mailRequest.Candidate));

            var hrm = await Task.Run(()=>userRepository.Read(hrmId));

            var interviewer = await Task.Run(()=>userRepository.Read(mailRequest.Interviewer));

            var mail = ChangeFormInterviewDateForInterviewer(hrm, interviewer,candidate, 
                mailRequest.Date, mailRequest.Skill);

            return mail;
        }

        public async Task<Mail> CancelInterviewForCandidate(MailRequest mailRequest, int hrmId)
        {
            var candidate = await Task.Run(()=>candidateRepository.Read(mailRequest.Candidate));

            var hrm = await Task.Run(()=>userRepository.Read(hrmId));

            var mail = CancelFormInterviewForCandidate(hrm, candidate);

            return mail;
        }

        public async Task<Mail> CancelInterviewForInterviewer(MailRequest mailRequest, int hrmId)
        {
            var candidate = await Task.Run(()=>candidateRepository.Read(mailRequest.Candidate));

            var hrm = await Task.Run(()=>userRepository.Read(hrmId));

            var interviewer = await Task.Run(()=>userRepository.Read(mailRequest.Interviewer));

            var mail = CancelFormInterviewForInterviewer(hrm, candidate, interviewer, mailRequest.Skill);

            return mail;
        }

        public Task Send(Mail mail)
        {
            return Task.Run
            (
                () =>
                {
                    var from = new MailAddress(senderEmail, senderName);

                    var to = new MailAddress(mail.Recipient);

                    var m = new MailMessage(from, to)
                    {
                        Subject = mail.Subject,

                        Body = mail.Body
                    };

                    var smtp = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential(senderEmail, senderPassword),

                        EnableSsl = true
                    };

                    smtp.Send(m);
                }
            );
        }

        public Mail AddFormInterviewForCandidate(User hrm, Candidate candidate, string skill, DateTime date)
        {
            var mail = new Mail
            {
                Recipient = candidate.Contact?.Email,

                Subject = "Приглашение на собеседование",

                Body = $"Здравствуйте, {candidate.FirstNameRus} {candidate.LastNameRus}!\n" +
                       "Мы хотели бы пригласить Вас на собеседование для " +
                       "дальнейшей возможности принятия на работу в нашей компании." +
                       $"Собеседовние будет проходить по направлению: {skill}.\n" +
                       $"Дата собеседования: {date}.\n" +
                       $"Пожалуйста пришлите свой ответ на {hrm.Email}.\n" +
                       $"По всем вопросам и уточнению времени и места также обращайтесь на {hrm.Email}.\n" +
                       $"С уважением компания Exadel, HRM {hrm.Name}!"
            };

            return mail;
        }

        public Mail ChangeFormInterviewDateForCandidate(User hrm, Candidate candidate, DateTime date)
        {
            var mail = new Mail
            {
                Recipient = candidate.Contact?.Email,

                Subject = "Изменение времени собеседования",

                Body = $"Здравствуйте, {candidate.FirstNameRus} {candidate.LastNameRus}!\n" +
                       $"Сообщаем Вам, что мы вынуждены перенести собеседование на {date}.\n" +
                       $"По всем вопросам обращайтесь на {hrm.Email}\n" +
                       $"С уважением компания Exadel, HRM {hrm.Name}!"
            };

            return mail;
        }

        public Mail CancelFormInterviewForCandidate(User hrm, Candidate candidate)
        {
            var mail = new Mail
            {
                Recipient = candidate.Contact?.Email,

                Subject = "Отмена собеседования",

                Body = $"Здравствуйте, {candidate.FirstNameRus} {candidate.LastNameRus}!\n" +
                       "Сообщаем Вам, что мы вынуждены отменить собеседование.\n" +
                       $"По всем вопросам обращайтесь на {hrm.Email}\n" +
                       $"С уважением компания Exadel, HRM {hrm.Name}!"
            };

            return mail;
        }

        private Mail AddFormInterviewForInterviewer(User hrm, User interviewer,
            string skill, DateTime date, Candidate candidate)
        {
            var mail = new Mail
            {
                Recipient = interviewer.Email,

                Subject = "Назначение на собеседование",

                Body = $"Здравствуйте, {interviewer.Name}!\n" +
                       $"Вас назначили провести интервью канидидату по направлению {skill}.\n" +
                       $"Дата собеседования: {date}.\n" +
                       $"Имя кандидата: {candidate.FirstNameRus} {candidate.LastNameRus}.\n" +
                       $"Для уточнения обращайтесь на {hrm.Email}.\n" +
                       $"С уважением, {hrm.Name}!"
            };

            return mail;
        }

        private Mail ChangeFormInterviewDateForInterviewer(User hrm, User interviewer, 
            Candidate candidate, DateTime date, string skill)
        {
            var mail = new Mail
            {
                Recipient = interviewer.Email,

                Subject = "Изменение времени собеседования",

                Body = $"Здравствуйте, {interviewer.Name}!\n" +
                       $"Время собеседования для кандидата {candidate.FirstNameRus} {candidate.LastNameRus}.\n" +
                       $"По {skill}\n" +
                       $"Новое время собеседования: {date}\n" +
                       $"С уважением, {hrm.Name}!"
            };

            return mail;
        }

        private Mail CancelFormInterviewForInterviewer(User hrm, Candidate candidate, User interviewer, string skill)
        {
            var mail = new Mail
            {
                Recipient = candidate.Contact?.Email,

                Subject = "Отмена собеседования",

                Body = $"Здравствуйте, {interviewer.Name}!\n" +
                       $"Собеседование назначеное для кандидата {candidate.FirstNameRus} {candidate.LastNameRus}.\n" +
                       $"По {skill}\n" +
                       $"С уважением, {hrm.Name}!"
            };

            return mail;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
