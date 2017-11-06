using BusinessLogic.DBContext;
using BusinessLogic.Helpers;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Classes
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<ICollection<Notification>> GetUnseen(int userId, int skip, int amount)
        {
            var filtered = await Task.Run(() => notificationRepository.Find(n => n.User == userId && n.Active == NotificationStates.Unseen));

            var result = await Task.Run(() => notificationRepository.PagingInFound(skip, amount, n => n.Id));

            return result.ToList();
        }

        public async Task<ICollection<Notification>> GetAll(int userId, int skip, int amount)
        {
            var filtered = await Task.Run(() => notificationRepository.Find(n => n.User == userId));

            var result = await Task.Run(() => notificationRepository.PagingInFound(skip, amount, n => n.Id));

            return result.ToList();
        }

        public async Task<ICollection<Notification>> GetAllOfType(int userId, int type, int skip, int amount)
        {
            var filtered = await Task.Run(() => notificationRepository.Find(n => n.User == userId && n.Type == type));

            var result = await Task.Run(() => notificationRepository.PagingInFound(skip, amount, n => n.Id));

            return result.ToList();
        }

        public async Task<ICollection<Notification>> GetUnseenOfType(int userId, int type, int skip, int amount)
        {
            var filtered = await Task.Run
            (
                () => notificationRepository.Find
                (
                    n => n.User == userId && n.Type == type && n.Active == NotificationStates.Unseen
                )
            );

            var result = await Task.Run(() => notificationRepository.PagingInFound(skip, amount, n => n.Id));

            return result.ToList();
        }

        public async Task<Notification> CreateNotification(int userId, int type, ICollection<Event> events)
        {
            var notification = new Notification
            {
                Active = NotificationStates.Unseen,

                Date = DateTime.UtcNow,

                Type = type,

                User = userId,

                Events = events
            };

            var res = await Task.Run(() => notificationRepository.Create(notification));

            return res;
        }

        public async Task MarkAsRead(int userId, List<int> ids)
        {
            var notifications = await Task.Run(() => notificationRepository.Find(n => n.User == userId && ids.Contains(n.Id)));

            await Task.Run(() =>
            {
                foreach (var notification in notifications)
                {
                    notification.Active = false;
                }
            });
        }

        public async Task<int> GetUnreadAmount(int userId)
        {
            var notifications = await Task.Run(() => notificationRepository.ReadAll());

            var requiredNotifications = notifications.Where(n => n.User == userId)
                .Where(n => n.Active == NotificationStates.Unseen);

            return requiredNotifications.Count(); 
        }
    }
}
