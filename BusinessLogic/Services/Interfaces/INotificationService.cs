using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface INotificationService
    { 
        Task MarkAsRead(int userId, List<int> ids);

        Task<ICollection<Notification>> GetAll(int userId, int skip, int amount);

        Task<ICollection<Notification>> GetAllOfType(int userId, int type, int skip, int amount);

        Task<ICollection<Notification>> GetUnseenOfType(int userId, int type, int skip, int amount);

        Task<ICollection<Notification>> GetUnseen(int userId, int skip, int amount);

        Task<Notification> CreateNotification(int userId, int type, ICollection<Event> events);

        Task<int> GetUnreadAmount(int userId);
    }
}
