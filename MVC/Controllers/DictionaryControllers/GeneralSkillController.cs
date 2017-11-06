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
    public class GeneralSkillController : ApiController
    {
        private readonly IDictionaryService<GeneralSkill> service;

        public GeneralSkillController(IDictionaryService<GeneralSkill> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/generalskill")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var generalSkills = await service.GetAll();

            var result = Mapper.Map<ICollection<GeneralSkill>, ICollection<GeneralSkillDTO>>(generalSkills);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
