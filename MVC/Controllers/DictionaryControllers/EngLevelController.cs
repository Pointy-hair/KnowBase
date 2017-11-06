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
    public class EngLevelController : ApiController
    {
        private readonly IDictionaryService<EngLevel> service;

        public EngLevelController(IDictionaryService<EngLevel> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/englevel")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var engLevels = await service.GetAll();

            var result = Mapper.Map<ICollection<EngLevel>, ICollection<EngLevelDTO>>(engLevels);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
