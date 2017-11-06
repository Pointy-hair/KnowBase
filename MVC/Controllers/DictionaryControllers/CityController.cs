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
    public class CityController : ApiController
    {
        private readonly IDictionaryService<City> service;

        public CityController(IDictionaryService<City> service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/city")]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var cities = await service.GetAll();

            var result = Mapper.Map<ICollection<City>, ICollection<CityDTO>>(cities);

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
