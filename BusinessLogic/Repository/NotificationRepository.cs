using BusinessLogic.DBContext;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
