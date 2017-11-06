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
    public class PrevJobContactsRepository : Repository<CandidatePrevJobsContact>, IUpdateRepository<CandidatePrevJobsContact>, IPrevJobContactsRepository
    {
        public PrevJobContactsRepository(IUnitOfWork context) : base(context)
        {
        }
        public CandidatePrevJobsContact Update(CandidatePrevJobsContact job)
        {
            CandidatePrevJobsContact oldJobContact = Read(job.Id);
            oldJobContact.CompanyName = job.CompanyName;
            oldJobContact.Position = job.Position;

            return oldJobContact;
        }
    }
}
