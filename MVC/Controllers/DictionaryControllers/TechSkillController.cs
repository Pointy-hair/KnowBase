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
    public class TechSkillController : ApiController
    {
        private readonly IDictionaryService<TechSkill> service;

        public TechSkillController(IDictionaryService<TechSkill> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/techskill")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var techSkills = await service.GetAll();

            var result = Mapper.Map<ICollection<TechSkill>, ICollection<TechSkillDTO>>(techSkills);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
