using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Repository
{
    public class GeneralInterviewRepository : Repository<GeneralInterview>, IUpdateRepository<GeneralInterview>, 
        IGeneralInterviewRepository
    {
        public GeneralInterviewRepository(IUnitOfWork context) : base(context)
        {
        }

        public GeneralInterview Update(GeneralInterview updateInterview)
        {
            var oldInterview = dbSet.Find(updateInterview.Id);

            if (updateInterview.Date != null)
            {
                oldInterview.Date = updateInterview.Date;
            }
            if (updateInterview.EndDate != null)
            {
                oldInterview.EndDate = updateInterview.EndDate;
            }
            if (updateInterview.EngLevel != null)
            {
                oldInterview.EngLevel = updateInterview.EngLevel;
            }
            if (updateInterview.Commentary != null)
            {
                oldInterview.Commentary = updateInterview.Commentary;
            }

            if (updateInterview.Feedback != null)
            {
                oldInterview.Feedback = updateInterview.Feedback;
            }

            if (updateInterview.Interviewer != null)
            {
                oldInterview.Interviewer = updateInterview.Interviewer;
            }

            oldInterview.Status = updateInterview.Status;

            if (updateInterview.EngLevel != 0)
            {
                oldInterview.EngLevel = updateInterview.EngLevel;
            }

            if (updateInterview.SalaryExpectations != 0)
            {
                oldInterview.SalaryExpectations = updateInterview.SalaryExpectations;
            }

            if (updateInterview.WhenCanStart != DateTime.MinValue)
            {
                oldInterview.WhenCanStart = updateInterview.WhenCanStart;
            }

            oldInterview.ReadyForBusinessTrips = updateInterview.ReadyForBusinessTrips;

            oldInterview.JobChangeReason = updateInterview.JobChangeReason;

            oldInterview.Interests = updateInterview.Interests;

            return oldInterview;
        }
    }
}
