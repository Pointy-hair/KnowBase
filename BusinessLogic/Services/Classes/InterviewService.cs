using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using BusinessLogic.Services.Tools;
using BusinessLogic.Helpers;

namespace BusinessLogic.Services.Classes
{
    public class InterviewService : IInterviewService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ITechInterviewRepository techRepository;

        private readonly ICandidateRepository candidateRepository;

        private readonly IGeneralInterviewRepository generalRepository;

        private readonly IDictionaryRepository<CandidateStatus> statusRepository;

        public InterviewService(IUnitOfWork unitOfWork, 
            ITechInterviewRepository techRepository, 
            ICandidateRepository candidateRepository, 
            IGeneralInterviewRepository generalRepository, 
            IDictionaryRepository<CandidateStatus> statusRepository )
        {
            this.unitOfWork = unitOfWork;
            this.techRepository = techRepository;
            this.candidateRepository = candidateRepository;
            this.generalRepository = generalRepository;
            this.statusRepository = statusRepository;
        }

        private void CompareInterviewDate(IInterview interview)
        {
            if (interview.Date == null && interview.EndDate == null)
            {
                return;
            }

            var candidate = candidateRepository.Read(interview.Candidate);

            bool Intersection(IInterview x) => (interview.Id!= x.Id) &&
                ((interview.Date > x.Date && interview.Date < x.EndDate)
                || (interview.EndDate < x.EndDate && interview.EndDate > x.Date));

            var generalInterviews = candidate.GeneralInterviews.Where(Intersection);

            if(generalInterviews!= null && generalInterviews.Count()!=0)
                throw new Exception("Not valid time");

            var techInterviews = candidate.TechInterviews.Where(Intersection);

            if(techInterviews!= null && techInterviews.Count()!=0)
                throw new Exception("Not valid time");
            
            if(interview.Id == 0 && interview.Status == InterviewStatuses.Active) return;

            if (interview.Date > candidate.CustomerInterviewDate &&
                interview.Date < candidate.CustomerInterviewEndDate)
            {
                throw new Exception("Not valid time");
            }

            if (interview.EndDate < candidate.CustomerInterviewEndDate &&
                interview.EndDate > candidate.CustomerInterviewDate)
            {
                throw new Exception("Not valid time");
            }
        }

        public async Task<TechInterview> AddTechInterview(TechInterview techInterview)
        {
            CompareInterviewDate(techInterview);

            return await Task.Run(async() =>
            {
                techInterview.Status = InterviewStatuses.Active;

                techRepository.Create(techInterview);

                var candidate = await Task.Run(() => candidateRepository.Read(techInterview.Candidate))
                .ConfigureAwait(false);

                candidate.Status = await Task.Run(() => statusRepository.Find(x => x.Name == "Interview").First().Id)
                .ConfigureAwait(false);

                candidate.TechInterviewStatus = true;

                return techInterview;
            });
        }

        public async Task<TechInterview> UpdateTechInterview(TechInterview techInterview)
        {
            CompareInterviewDate(techInterview);

            var result = await Task.Run(() => techRepository.Update(techInterview));

            return result;
        }

        public async Task<ICollection<TechInterview>> GetTechInterviewsByCandidate(int id)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            return candidate.TechInterviews.ToList();
        }

        public async Task<ICollection<GeneralInterview>> GetGeneralInterviewsByCandidate(int id)
        {
            var candidate = await Task.Run(() => candidateRepository.Read(id));

            return candidate.GeneralInterviews.ToList();
        }

        public async Task<GeneralInterview> AddGeneralInterview(GeneralInterview generalInterview)
        {
            CompareInterviewDate(generalInterview);

            return await Task.Run(async () =>
            {
                generalInterview.Status = InterviewStatuses.Active;

                generalRepository.Create(generalInterview);

                var candidate = await Task.Run(()=>candidateRepository.Read(generalInterview.Candidate))
                .ConfigureAwait(false);

                candidate.Status = await Task.Run(()=>statusRepository.Find(x => x.Name == "Interview").First().Id)
                .ConfigureAwait(false);

                candidate.GeneralInterviewStatus = InterviewStatuses.Active;

                return generalInterview;
            });
        }

        public async Task<GeneralInterview> UpdateGeneralInterview(GeneralInterview generalInterview)
        {
            CompareInterviewDate(generalInterview);

            var result = await Task.Run(() => generalRepository.Update(generalInterview));

            return result;
        }

        public Task AddCustomerInterview(int candidateId, DateTime date, DateTime endDate)
        {
            IInterview interview = new GeneralInterview
            {
                Candidate = candidateId,
                Date = date,
                EndDate = endDate,
                Status = InterviewStatuses.Active
            };
            CompareInterviewDate(interview);

            return Task.Run(() =>
            {
                Candidate candidate = candidateRepository.Read(candidateId);
                candidate.CustomerInterviewDate = date;
                candidate.CustomerInterviewEndDate = endDate;
                candidate.CustomerInterviewStatus = InterviewStatuses.Active;
            });
        }

        public Task UpdateCustomerInterview(int candidateId, DateTime? date, DateTime? endDate)
        {
            IInterview interview = new GeneralInterview
            {
                Candidate = candidateId,
                Date = date,
                EndDate = endDate,
                Status = InterviewStatuses.Active
            };
            CompareInterviewDate(interview);
            return Task.Run(() =>
            {
                Candidate candidate = candidateRepository.Read(candidateId);
                if (date != null)
                {
                    candidate.CustomerInterviewDate = date;
                }
                if (endDate != null)
                {
                    candidate.CustomerInterviewEndDate = endDate;
                }
            });
        }

        public Task CustomerInterviewClose(int candidateId)
        {
            return Task.Run(() =>
            {
                Candidate candidate = candidateRepository.Read(candidateId);
                candidate.CustomerInterviewStatus = InterviewStatuses.Closed;
            });
        }

        public async Task<GeneralInterview> SetGeneralInterviewFeedback(GeneralInterview generalInterview)
        {
            generalInterview.Status = InterviewStatuses.Closed;

            generalInterview.EndDate = DateTime.UtcNow;

            var result = await Task.Run(() => generalRepository.Update(generalInterview));

            return result;
        }

        public async Task<TechInterview> SetTechInterviewFeedback(TechInterview techInterview)
        {
            techInterview.EndDate = DateTime.UtcNow;

            techInterview.Status = InterviewStatuses.Closed;

            var result = await Task.Run(() => techRepository.Update(techInterview));

            return result;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
