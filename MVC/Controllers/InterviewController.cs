using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using MVC.Models;
using MVC.Models.InDTO;
using MVC.Models.OutDTO;
using BusinessLogic.Services;
using BusinessLogic.Services.Tools;
using MVC.Hubs;
using MVC.Authorization;
using BusinessLogic.Helpers;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class InterviewController : ApiController
    {
        private readonly IInterviewService interviewService;

        private readonly IEventService eventService;

        private readonly INotificationService notificationService;

        private readonly IUnitOfWork unitOfWork;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public InterviewController(IEventService eventService, IInterviewService interviewService,
            IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            this.interviewService = interviewService;

            this.eventService = eventService;

            this.notificationService = notificationService;

            this.unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [Route("api/interview/tech/bycandidate/{id}")]
        public async Task<HttpResponseMessage> GetTech(HttpRequestMessage request, int id)
        {
            try
            {
                var techInterviews = await interviewService.GetTechInterviewsByCandidate(id);

                var interviewDTO = Mapper.Map<ICollection<TechInterview>, ICollection<TechInterviewDTO>>(techInterviews);

                return request.CreateResponse(HttpStatusCode.OK, interviewDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/interview/tech/add")]
        public async Task<HttpResponseMessage> AddTechInterview(HttpRequestMessage request, [FromBody]TechInterviewInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var interview = Mapper.Map<TechInterviewInDTO, TechInterview>(value);

                interview.HRM = userId;

                var createdInterview = await interviewService.AddTechInterview(interview);

                var e = await eventService.RegisterTechInterview(createdInterview, userId);

                unitOfWork.Save();

                if (createdInterview.Interviewer.HasValue)
                {
                    var notification = await notificationService.CreateNotification(createdInterview.Interviewer.Value, NotificationTypes.Interview, new List<Event> { e });

                    if (NotificationsHub.IsConnected(createdInterview.Interviewer.Value))
                    {
                        await NotificationsHub.PushNotification(notification);
                    }
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/tech/update")]
        public async Task<HttpResponseMessage> UpdateTech(HttpRequestMessage request, [FromBody]InterviewUpdateDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var techInterview = Mapper.Map<InterviewUpdateDTO, TechInterview>(value);

                techInterview.HRM = userId;

                var eventsForNotification = await eventService.RegisterTechInterviewUpdate(techInterview, userId);

                var updatedInterview = await interviewService.UpdateTechInterview(techInterview);

                unitOfWork.Save();

                if (updatedInterview.Interviewer.HasValue)
                {
                    var notification = await notificationService.CreateNotification(updatedInterview.Interviewer.Value,
                        NotificationTypes.Update, eventsForNotification);

                    if (NotificationsHub.IsConnected(updatedInterview.Interviewer.Value))
                    {
                        await NotificationsHub.PushNotification(notification);
                    }
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/tech/update/feedback")]
        public async Task<HttpResponseMessage> SetFeedbackTech(HttpRequestMessage request, [FromBody]TechInterviewFeedbackInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var techInterview = Mapper.Map<TechInterviewFeedbackInDTO, TechInterview>(value);

                var updatedInterview = await interviewService.SetTechInterviewFeedback(techInterview);

                var events = await eventService.RegisterTechInterviewFeedback(updatedInterview, userId);

                unitOfWork.Save();

                await notificationService.MarkAsRead(userId, new List<int> { value.NotificationId });

                var notification = await notificationService.CreateNotification(updatedInterview.HRM,
                    NotificationTypes.Update, events);

                if (NotificationsHub.IsConnected(updatedInterview.HRM))
                {
                    await NotificationsHub.PushNotification(notification);
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/interview/general/bycandidate/{id}")]
        public async Task<HttpResponseMessage> GetGeneral(HttpRequestMessage request, int id)
        {
            try
            {
                var generalInterview = await interviewService.GetGeneralInterviewsByCandidate(id).ConfigureAwait(false);

                var interviewDTO = Mapper.Map<ICollection<GeneralInterview>, ICollection<GeneralInterviewDTO>>(generalInterview);

                return request.CreateResponse(HttpStatusCode.OK, interviewDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/interview/general/add")]
        public async Task<HttpResponseMessage> AddGeneralInterview(HttpRequestMessage request, [FromBody]GeneralInterviewInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var interview = Mapper.Map<GeneralInterviewInDTO, GeneralInterview>(value);

                interview.HRM = userId;

                var createdInterview = await interviewService.AddGeneralInterview(interview);

                var e = await eventService.RegisterGeneralInterview(createdInterview, userId);

                unitOfWork.Save();

                if (createdInterview.Interviewer.HasValue)
                {
                    var notification = await notificationService.CreateNotification(createdInterview.Interviewer.Value,
                        NotificationTypes.Interview, new List<Event> { e });

                    if (NotificationsHub.IsConnected(createdInterview.Interviewer.Value))
                    {
                        await NotificationsHub.PushNotification(notification);
                    }
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/general/update")]
        public async Task<HttpResponseMessage> UpdateGeneral(HttpRequestMessage request, [FromBody]InterviewUpdateDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var generalInterview = Mapper.Map<InterviewUpdateDTO, GeneralInterview>(value);

                generalInterview.HRM = userId;

                var events = await eventService.RegisterGeneralInterviewUpdate(generalInterview, userId);

                var updatedInterview = await interviewService.UpdateGeneralInterview(generalInterview);

                unitOfWork.Save();

                if (updatedInterview.Interviewer.HasValue)
                {
                    var notification = await notificationService.CreateNotification(updatedInterview.Interviewer.Value,
                        NotificationTypes.Update, events);

                    if (NotificationsHub.IsConnected(updatedInterview.Interviewer.Value))
                    {
                        await NotificationsHub.PushNotification(notification);
                    }
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/general/feedback")]
        public async Task<HttpResponseMessage> SetFeedbackGeneral(HttpRequestMessage request, [FromBody]GeneralInterviewFeedbackInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var generalInterview = Mapper.Map<GeneralInterviewFeedbackInDTO, GeneralInterview>(value);

                var updatedInterview = await interviewService.SetGeneralInterviewFeedback(generalInterview);

                var events = await eventService.RegisterGeneralInterviewFeedback(updatedInterview, userId);

                unitOfWork.Save();

                await notificationService.MarkAsRead(userId, new List<int> { value.NotificationId });

                var notification = await notificationService.CreateNotification(updatedInterview.HRM,
                    NotificationTypes.Update, events);

                if (NotificationsHub.IsConnected(updatedInterview.HRM))
                {
                    await NotificationsHub.PushNotification(notification);
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/interview/customer/add")]
        public async Task<HttpResponseMessage> AddCustomerInterview(HttpRequestMessage request, [FromBody]CustomerIntreviewInDTO value)
        {
            try
            {
                await interviewService.AddCustomerInterview(value.Candidate, value.Date, value.EndDate);

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/customer/update")]
        public async Task<HttpResponseMessage> UpdateCustomer(HttpRequestMessage request, [FromBody]InterviewUpdateDTO value)
        {
            try
            {
                await interviewService.UpdateCustomerInterview(value.Candidate.Value,
                    value.Date, value.EndDate);

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/interview/customer/feedback")]
        public async Task<HttpResponseMessage> CloseCustomer(HttpRequestMessage request, int id)
        {
            try
            {
                await interviewService.CustomerInterviewClose(id);

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }

}
