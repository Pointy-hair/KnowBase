using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Services.Interfaces;
using MVC.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class PrevJobContactsController : ApiController
    {
        private readonly IPrevJobsContactsService prevJobContactsService;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PrevJobContactsController(IPrevJobsContactsService prevJobContactsService)
        {
            this.prevJobContactsService = prevJobContactsService;
        }

        [HttpGet]
        [Route("api/candidateprevjob/bycandidateid/{id}")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            try
            {
                var jobContacts = await prevJobContactsService.GetByCandidateId(id);

                var jobsContactDTO = Mapper.Map<List<CandidatePrevJobsContact>, List<CandidatePrevJobDTO>>(jobContacts);

                return request.CreateResponse(HttpStatusCode.OK, jobsContactDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
