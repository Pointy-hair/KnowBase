using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface INotificationRepository : IFindRepository<Notification>,
        IPagableRepository<Notification>, IReadRepository<Notification>, ICreateRepository<Notification>
    {
    }
}
