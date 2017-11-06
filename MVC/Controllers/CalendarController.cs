using BusinessLogic.Unit_of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.Services.Interfaces;
using MVC.Models;
using MVC.Models.OutDTO;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class CalendarController : ApiController
    {

        private readonly IGoogleCalendarService googleCalendarService;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CalendarController(IGoogleCalendarService googleCalendarService)
        {
            this.googleCalendarService = googleCalendarService;
        }

        [HttpGet]
        [Route("api/calendar")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int interviewId, string type, int? candidateId)
        {
            try
            {
                var calendarEvent = googleCalendarService.DefaultEvent(interviewId, type, candidateId);

                var eventOut = Mapper.Map<CalendarEvent, CalendarEventDTO>(calendarEvent);

                return request.CreateResponse(HttpStatusCode.OK, eventOut);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { interviewId, type, candidateId }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/calendar")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, [FromBody]CalendarEventDTO value)
        {
            try
            {
                var calendarEvent = Mapper.Map<CalendarEventDTO, CalendarEvent>(value);

                var htmlLink = googleCalendarService.AddEvent(calendarEvent);

                return request.CreateResponse(HttpStatusCode.OK, htmlLink);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
