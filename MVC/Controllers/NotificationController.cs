using AutoMapper;
using BusinessLogic.Helpers;
using BusinessLogic.Services;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using MVC.Authorization;
using MVC.Hubs;
using MVC.Models.OutDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2, 1")]
    public class NotificationController : ApiController
    {
        private readonly INotificationService notificationService;

        private readonly IUnitOfWork unitOfWork;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public NotificationController(INotificationService notificationService, IUnitOfWork unitOfWork)
        {
            this.notificationService = notificationService;

            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/notification/all")]
        [Authorize(Roles = "3, 2, 1")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, int skip, int amount)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var notifications = await notificationService.GetAll(userId, skip, amount);

                var result = Mapper.Map<ICollection<NotificationDTO>>(notifications);

                for (var i = 0; i < notifications.Count; i++)
                {
                    result.ElementAt(i).Title = NotificationInfoStringBuilder.GetNotificationTitle(notifications.ElementAt(i));
                }

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/notification/unseen")]
        [Authorize(Roles = "3, 2, 1")]
        public async Task<HttpResponseMessage> GetUnseen(HttpRequestMessage request, int skip, int amount)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var notifications = await notificationService.GetUnseen(userId, skip, amount);

                var result = Mapper.Map<ICollection<NotificationDTO>>(notifications);

                for (var i = 0; i < notifications.Count; i++)
                {
                    result.ElementAt(i).Title = NotificationInfoStringBuilder.GetNotificationTitle(notifications.ElementAt(i));
                }

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/notification/unseenoftype")]
        [Authorize(Roles = "3, 2, 1")]
        public async Task<HttpResponseMessage> GetUnseenOfType(HttpRequestMessage request, int type, int skip, int amount)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var notifications = await notificationService.GetUnseenOfType(userId, type, skip, amount);

                var result = Mapper.Map<ICollection<NotificationDTO>>(notifications);

                for (var i = 0; i < notifications.Count; i++)
                {
                    result.ElementAt(i).Title = NotificationInfoStringBuilder.GetNotificationTitle(notifications.ElementAt(i));
                }

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { type, skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/notification/alloftype")]
        [Authorize(Roles = "3, 2, 1")]
        public async Task<HttpResponseMessage> GetAllOfType(HttpRequestMessage request, int type, int skip, int amount)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var notifications = await notificationService.GetAllOfType(userId, type, skip, amount);

                var result = Mapper.Map<ICollection<NotificationDTO>>(notifications);

                for (var i = 0; i < notifications.Count; i++)
                {
                    result.ElementAt(i).Title = NotificationInfoStringBuilder.GetNotificationTitle(notifications.ElementAt(i));
                }

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { type, skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/notification/update/status")]
        [Authorize(Roles = "3, 2, 1")]
        public async Task<HttpResponseMessage> MarkAsRead(HttpRequestMessage request, List<int> ids)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                await notificationService.MarkAsRead(userId, ids);

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(ids));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}