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
    public class TechInterviewRepository : Repository<TechInterview>, IUpdateRepository<TechInterview>, ITechInterviewRepository
    {
        public TechInterviewRepository(IUnitOfWork context) : base(context)
        {
        }

        public TechInterview Update(TechInterview updateInterview)
        {
            var oldInterview = dbSet.Find(updateInterview.Id);

            if (updateInterview.Interviewer != null)
            {
                oldInterview.Interviewer = updateInterview.Interviewer;
            }

            if (updateInterview.Date != null)
            {
                oldInterview.Date = updateInterview.Date;
            }

            if (updateInterview.EndDate != null)
            {
                oldInterview.EndDate = updateInterview.EndDate;
            }

            if (updateInterview.Mark != null)
            {
                oldInterview.Mark = updateInterview.Mark;
            }
            if (updateInterview.Commentary != null)
            {
                oldInterview.Commentary = updateInterview.Commentary;
            }

            if (updateInterview.Feedback != null)
            {
                oldInterview.Feedback = updateInterview.Feedback;
            }

            oldInterview.Status = updateInterview.Status;

            return oldInterview;
        }
    }
}
