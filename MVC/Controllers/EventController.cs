using BusinessLogic.Services;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Tools;
using MVC.Models.OutDTO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using BusinessLogic.Services.Interfaces;
using System;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class EventController : ApiController
    {
        private readonly IEventService eventService;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        [Route("api/event")]
        public async Task<HttpResponseMessage> GetPreview(HttpRequestMessage request, int skip, int amount)
        {
            try
            {
                var events = await eventService.Paging(skip, amount).ConfigureAwait(false);

                var result = Mapper.Map<IEnumerable<Event>, List<EventDTO>>(events);

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { skip, amount }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/event/search")]
        public async Task<HttpResponseMessage> Search(HttpRequestMessage request, EventSearchOptions value)
        {
            try
            {
                var events = await eventService.Find(value).ConfigureAwait(false);

                var result = Mapper.Map<IEnumerable<Event>, List<EventDTO>>(events);

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}