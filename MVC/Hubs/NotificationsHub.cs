using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MVC.Authorization;
using System.Security.Claims;
using BusinessLogic.DBContext;
using BusinessLogic.Helpers;
using BusinessLogic.Services.Interfaces;
using MVC.App_Start;
using Microsoft.Practices.Unity;

namespace MVC.Hubs
{
    [SignalRAuthorize]
    [HubName("notifications")]
    public class NotificationsHub : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> connections = 
            new Dictionary<string, HashSet<string>>();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override Task OnConnected()
        {
            object userIdContainer;

            var userIsFound = Context.Request.Environment.TryGetValue("user", out userIdContainer);

            if (userIsFound)
            {
                var userId = GetUserId(userIdContainer as ClaimsPrincipal);

                lock(connections)
                {
                    HashSet<string> connectionIds;

                    if (!connections.TryGetValue(userId, out connectionIds))
                    {
                        connectionIds = new HashSet<string>();

                        connections.Add(userId, connectionIds);
                    }

                    lock (connectionIds)
                    {
                        connectionIds.Add(Context.ConnectionId);

                        logger.Info("User " + userId + " connected on " + Context.ConnectionId);
                    }
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            object temp;

            var userIsFound = Context.Request.Environment.TryGetValue("user", out temp);

            if (userIsFound)
            {
                var userId = GetUserId(temp as ClaimsPrincipal);

                lock (connections)
                {
                    HashSet<string> connectionIds;

                    if (!connections.TryGetValue(userId, out connectionIds))
                    {
                        return base.OnDisconnected(stopCalled);
                    }

                    lock (connectionIds)
                    {
                        connectionIds.Remove(Context.ConnectionId);

                        logger.Info("User " + userId + " disconnected on " + Context.ConnectionId);

                        if (connectionIds.Count == 0)
                        {
                            connections.Remove(userId);
                        }
                    }
                }
            }           

            return base.OnDisconnected(stopCalled);
        }

        public static bool IsConnected(int userId)
        {
            lock (connections)
            {
                if (connections.ContainsKey(userId.ToString()))
                {
                    return true;
                }

                return false;
            }
        }

        public static async Task PushNotification(Notification notification)
        {
            await Task.Run
            (
                () =>
                {
                    HashSet<string> connectionIds;

                    var connectionIsFound = connections.TryGetValue(notification.User.ToString(), out connectionIds);

                    if (connectionIsFound)
                    {
                        foreach (var connectionId in connectionIds)
                        {
                            logger.Info("Pushed notification to " + notification.User.ToString() + " on connectionId " + connectionId);
                        }

                        var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();

                        context.Clients.Clients(connectionIds.ToList())
                            .GetNotification(NotificationInfoStringBuilder.GetSnackString(notification));
                    }
                }
            );
        }

        public async Task SendUnreadAmount()
        {
            object userIdContainer;

            var userIsFound = Context.Request.Environment.TryGetValue("user", out userIdContainer);

            if (userIsFound)
            {
                var userId = GetUserId(userIdContainer as ClaimsPrincipal);

                var notificationService = UnityConfig.GetConfiguredContainer().Resolve<INotificationService>();

                var sendback = await notificationService.GetUnreadAmount(int.Parse(userId));

                HashSet<string> connectionIds;

                var connectionIsFound = connections.TryGetValue(userId.ToString(), out connectionIds);

                if (connectionIsFound)
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();

                    context.Clients.Clients(connectionIds.ToList()).GetUnreadAmount(sendback);
                }
            }
        }

        private string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();

            return id;
        }
    }
}