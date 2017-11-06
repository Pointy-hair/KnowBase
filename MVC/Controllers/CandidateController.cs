using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticServices.IElasticServices;
using BusinessLogic.Unit_of_Work;
using MVC.Models.InDTO;
using BusinessLogic.Services.Interfaces;
using MVC.Hubs;
using BusinessLogic.Helpers;
using MVC.Models.OutDTO;
using MVC.Authorization;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class CandidateController : ApiController
    {
        private readonly ICandidateService candidateService;

        private readonly IUnitOfWork unitOfWork;

        private readonly IEventService eventService;

        private readonly INotificationService notificationService;

        private readonly ICandidateElasticService candidateElasticService;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CandidateController(IEventService eventService, ICandidateService candidateService,
            INotificationService notificationService, IUnitOfWork unitOfWork,
            ICandidateElasticService candidateElasticService)
        {
            this.candidateElasticService = candidateElasticService;

            this.notificationService = notificationService;

            this.candidateService = candidateService;

            this.unitOfWork = unitOfWork;

            this.eventService = eventService;
        }


        [HttpPost]
        [Route("api/candidate/autosearch")]
        public async Task<HttpResponseMessage> AutoSelect(HttpRequestMessage request, [FromBody]AutoSearchInDTO value)
        {
            try
            {
                var candidates = await candidateService.AutoCandidatesByVacancy(value.Vacancy, value.Skip, value.Amount, value.Coefficient);

                var candidatesDTO = Mapper.Map<ICollection<Candidate>, ICollection<CandidatePreviewDTO>>(candidates);

                return request.CreateResponse(HttpStatusCode.OK, candidatesDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/candidate/{id}")]
        public async Task<HttpResponseMessage> GetById(HttpRequestMessage request, int id)
        {
            try
            {
                var candidate = await candidateService.GetById(id);

                var candidateDTO = Mapper.Map<Candidate, CandidateDTO>(candidate);

                return request.CreateResponse(HttpStatusCode.OK, candidateDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/candidate/byids")]
        public async Task<HttpResponseMessage> GetPreviewsByIds(HttpRequestMessage request, List<int> ids)
        {
            try
            {
                var idsHashSet = new HashSet<int>(ids);

                var candidates = await candidateService.GetCandidatesByIds(ids);

                var candidatesPreviews = Mapper.Map<ICollection<Candidate>, ICollection<CandidatePreviewDTO>>(candidates);

                return request.CreateResponse(HttpStatusCode.OK, candidatesPreviews);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(ids));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/candidate/update/vacancies/add")]
        public async Task<HttpResponseMessage> AssignVacancies(HttpRequestMessage request, CandidatesVacanciesInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var result = await candidateService.AssignVacancies(value.Vacancies, value.Candidates, userId);

                unitOfWork.Save();

                foreach (var item in result.Events)
                {
                    var notification = await notificationService.CreateNotification(item.Key,
                        NotificationTypes.Update, item.Value);

                    if (NotificationsHub.IsConnected(item.Key))
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

        [HttpGet]
        [Route("api/candidate/update/vacancies/delete")]
        public async Task<HttpResponseMessage> UnassignVacancy(HttpRequestMessage request, int candidateId, int vacancyId)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var e = await eventService.RegisterCandidateFromVacancyUnassignment(vacancyId, candidateId, userId);

                var updatedCandidate = await candidateService.UnassignVacancy(candidateId, vacancyId, userId);

                unitOfWork.Save();

                var notification = await notificationService.CreateNotification(updatedCandidate.HRM,
                    NotificationTypes.Update, new List<Event> { e });

                if (NotificationsHub.IsConnected(updatedCandidate.HRM))
                {
                    await NotificationsHub.PushNotification(notification);
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { candidateId, vacancyId }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/candidate/add")]
        public async Task<HttpResponseMessage> AddCandidate(HttpRequestMessage request, [FromBody]CandidateInputDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var candidate = Mapper.Map<CandidateInputDTO, Candidate>(value);

                candidate.HRM = userId;

                candidate.LastModifier = userId;

                var createdCandidate = await candidateService.Add(candidate, value.VacanciesIds)
                    .ConfigureAwait(false);

                await eventService.RegisterCandidate(createdCandidate, userId);

                unitOfWork.Save();

                var candidateElastic = Mapper.Map<Candidate, CandidateElasticModel>(createdCandidate);
                await candidateElasticService.AddCandidateElastic(candidateElastic).ConfigureAwait(false);

                return request.CreateResponse(HttpStatusCode.OK, createdCandidate.Id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/candidate/search")]
        public async Task<HttpResponseMessage> Search(HttpRequestMessage request, CandidateSearchInputDTO value)
        {
            try
            {
                var candidates = await candidateElasticService
                    .Find(value.Skip, value.Amount, value.SearchModel, value.SortModel).ConfigureAwait(false);

                var candidateDTO = Mapper.Map<IEnumerable<CandidateElasticModel>, ICollection<CandidatePreviewDTO>>(candidates);

                return request.CreateResponse(HttpStatusCode.OK, candidateDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/candidate/update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, [FromBody]CandidateInputDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var candidate = Mapper.Map<CandidateInputDTO, Candidate>(value);

                candidate.LastModifier = userId;

                var events = await eventService.RegisterCandidateUpdate(candidate, userId);

               var updatedCandidate = await candidateService.Update(candidate, value.VacanciesIds)
                    .ConfigureAwait(false);

                unitOfWork.Save();

                var candidateElasticModel = Mapper.Map<Candidate, CandidateElasticModel>(updatedCandidate);
                await candidateElasticService.UpdateCandidateElastic(candidateElasticModel);

                var notification = await notificationService.CreateNotification(updatedCandidate.HRM,
                    NotificationTypes.Update, events);

                if (NotificationsHub.IsConnected(updatedCandidate.HRM))
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

        [HttpPut]
        [Route("api/candidate/update/status")]
        public async Task<HttpResponseMessage> UpdateStatus(HttpRequestMessage request, [FromBody]StatusUpdateInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var e = await eventService.RegisterCandidateStatusUpdate(value.EntityId, value.Status, userId);

                var candidate = await candidateService.UpdateStatus(value.EntityId, value.Status, userId);

                unitOfWork.Save();

                await candidateElasticService.UpdateStatusElastic(value.EntityId, value.Status).ConfigureAwait(false);

                var notification = await notificationService.CreateNotification(candidate.HRM,
                    NotificationTypes.Update, new List<Event> { e });

                if (NotificationsHub.IsConnected(candidate.HRM))
                {
                    await NotificationsHub.PushNotification(notification);
                }

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}