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
    public class VacancyStatusController : ApiController
    {
        private readonly IDictionaryService<VacancyStatus> service;

        public VacancyStatusController(IDictionaryService<VacancyStatus> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/vacancystatus")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var vacancyStatuses = await service.GetAll();

            var result = Mapper.Map<ICollection<VacancyStatus>, ICollection<VacancyStatusDTO>>(vacancyStatuses);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
