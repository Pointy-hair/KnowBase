using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Services.Interfaces;
using MVC.Models;
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
    public class CandidateStatusController : ApiController
    {
        private readonly IDictionaryService<CandidateStatus> service;

        public CandidateStatusController(IDictionaryService<CandidateStatus> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/candidatestatus")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var candidateStatuses = await service.GetAll();

            var result = Mapper.Map<ICollection<CandidateStatus>, ICollection<CandidateStatusDTO>>(candidateStatuses);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
