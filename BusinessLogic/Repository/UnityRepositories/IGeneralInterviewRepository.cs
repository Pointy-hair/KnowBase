using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface IGeneralInterviewRepository: IUpdateRepository<GeneralInterview>, 
        ICreateRepository<GeneralInterview>, IReadRepository<GeneralInterview>
    {
    }
}
