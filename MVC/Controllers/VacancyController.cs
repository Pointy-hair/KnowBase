using System;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Unit_of_Work;
using MVC.Models.InDTO;
using MVC.Models.OutDTO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticRepository;
using BusinessLogic.ElasticSearch.ElasticServices.IElasticServices;
using MVC.Hubs;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Helpers;
using MVC.Authorization;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class VacancyController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IVacancyService vacancyService;

        private readonly IEventService eventService;

        private readonly INotificationService notificationService;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IVacancyElasticService vacancyElasticService;

        public VacancyController(IUnitOfWork unitOfWork, 
            IVacancyService vacancyService,
            IEventService eventService, 
            INotificationService notificationService,
            IVacancyElasticService vacancyElasticService)
        {
            this.vacancyElasticService = vacancyElasticService;

            this.vacancyService = vacancyService;

            this.eventService = eventService;

            this.notificationService = notificationService;

            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/vacancy/{id}")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            try
            {
                var vacancy = await vacancyService.Get(id).ConfigureAwait(false);

                var res = Mapper.Map<Vacancy, VacancyDTO>(vacancy);

                return request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/vacancy")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int skip, int amount)
        {
            try
            {
                var vacancy = await vacancyService.Get(skip, amount);

                var res = Mapper.Map<ICollection<Vacancy>, ICollection<VacancyPreviewDTO>>(vacancy);

                return request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/vacancy/search")]
        public async Task<HttpResponseMessage> Search(HttpRequestMessage request, [FromBody]VacancySearchInputDTO value)
        {
            try
            {
                var vacancies = await vacancyElasticService.Search(value.Skip, value.Amount, value.SearchModel, value.SortModel)
                    .ConfigureAwait(false);

                var result = Mapper.Map<IEnumerable<VacancyElasticModel>, IEnumerable<VacancyPreviewDTO>>(vacancies);

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/vacancy/add")]
        public async Task<HttpResponseMessage> Add(HttpRequestMessage request, [FromBody]VacancyInputDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var vacancy = Mapper.Map<VacancyInputDTO, Vacancy>(value);

                var createdVacancy = await vacancyService.Post(vacancy, value.Candidates, userId)
                    .ConfigureAwait(false);

                await eventService.RegisterVacancy(createdVacancy, userId);

                unitOfWork.Save();

                var elasticVacancy = Mapper.Map<Vacancy, VacancyElasticModel>(createdVacancy);
                await vacancyElasticService.AddVacancyElastic(elasticVacancy);

                return request.CreateResponse(HttpStatusCode.OK, createdVacancy.Id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/vacancy/update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, [FromBody]VacancyInputDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var vacancy = Mapper.Map<VacancyInputDTO, Vacancy>(value);

                var e = await eventService.RegisterVacancyUpdate(vacancy, userId);

                var updatedVacancy = await vacancyService.Update(vacancy, value.Candidates, userId)
                    .ConfigureAwait(false);

                unitOfWork.Save();

                var elasticVacancy = Mapper.Map<Vacancy, VacancyElasticModel>(updatedVacancy);
                await vacancyElasticService.UpdateVacancyElastic(elasticVacancy);

                var notification = await notificationService.CreateNotification(updatedVacancy.HRM,
                    NotificationTypes.Update, e);

                if (NotificationsHub.IsConnected(updatedVacancy.HRM))
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
        [Route("api/vacancy/update/status")]
        public async Task<HttpResponseMessage> UpdateStatus(HttpRequestMessage request, [FromBody]StatusUpdateInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var e = await eventService.RegisterVacancyStatusUpdate(value.EntityId, value.Status, userId);

                var vacancy = await vacancyService.UpdateStatus(value.EntityId, value.Status, userId);

                unitOfWork.Save();

                await vacancyElasticService.UpdateStatusElastic(value.EntityId, value.Status);

                var notification = await notificationService.CreateNotification(vacancy.HRM, NotificationTypes.Update, new List<Event> { e });

                if (NotificationsHub.IsConnected(vacancy.HRM))
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
        [Route("api/vacancy/byids")]
        public async Task<HttpResponseMessage> GetVacanciesById(HttpRequestMessage request, [FromBody]List<int> value)
        {
            try
            {
                var vacancies = await vacancyService.GetVacanciesById(value);

                var vacancyPreviewsDTO = Mapper.Map<ICollection<VacancyPreviewDTO>>(vacancies);

                return request.CreateResponse(HttpStatusCode.OK, vacancyPreviewsDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/vacancy/autosearch")]
        public async Task<HttpResponseMessage> AutoSearchForVacancies(HttpRequestMessage request, [FromBody]AutoSearchInDTO value)
        {
            try
            {
                var vacancies = await vacancyService.AutoVacanciesByCandidate(value.Candidate, value.Coefficient, value.Skip, value.Amount);

                var result = Mapper.Map<ICollection<VacancyPreviewDTO>>(vacancies);

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/vacancy/update/candidates/add")]
        public async Task<HttpResponseMessage> AssignCandidates(HttpRequestMessage request, [FromBody]CandidatesVacanciesInDTO value)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var result = await vacancyService.AssignCandidates(value.Vacancies, value.Candidates, userId);

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
                logger.Error(ex);

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/vacancy/update/candidates/delete")]
        public async Task<HttpResponseMessage> UnassignCandidate(HttpRequestMessage request, int vacancyId, int candidateId)
        {
            try
            {
                var userId = ContextParser.GetUserId(request.GetRequestContext());

                var updatedVacancy = await vacancyService.UnassignCandidate(vacancyId, candidateId, userId);

                var e = await eventService.RegisterCandidateFromVacancyUnassignment(vacancyId, candidateId, userId);

                unitOfWork.Save();

                var notification = await notificationService.CreateNotification(updatedVacancy.HRM,
                    NotificationTypes.Update, new List<Event> { e });

                if (NotificationsHub.IsConnected(updatedVacancy.HRM))
                {
                    await NotificationsHub.PushNotification(notification);
                }

                unitOfWork.Save();

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { vacancyId, candidateId }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}