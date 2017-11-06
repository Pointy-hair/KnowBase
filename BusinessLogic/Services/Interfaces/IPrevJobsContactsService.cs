using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;

namespace BusinessLogic.Services.Interfaces
{
    public interface IPrevJobsContactsService : IDisposable
    {
        Task<List<CandidatePrevJobsContact>> GetByCandidateId(int id);
    }
}
