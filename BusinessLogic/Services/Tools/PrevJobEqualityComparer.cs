using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;

namespace BusinessLogic.Services.Tools
{
    public class PrevJobEqualityComparer : IEqualityComparer<CandidatePrevJobsContact>
    {
        public bool Equals(CandidatePrevJobsContact first, CandidatePrevJobsContact second)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            var result = first.Id == second.Id;

            return result;
        }

        public int GetHashCode(CandidatePrevJobsContact job)
        {
            return job.Id.GetHashCode();
        }
    }
}
