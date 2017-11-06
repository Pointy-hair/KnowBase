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
    public class UserRoleController : ApiController
    {
        private readonly IDictionaryService<UserRole> service;

        public UserRoleController(IDictionaryService<UserRole> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/userrole")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var userRoles = await service.GetAll();

            var result = Mapper.Map<ICollection<UserRole>, ICollection<UserRoleDTO>>(userRoles);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
