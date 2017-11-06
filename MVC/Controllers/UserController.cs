using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using MVC.Models;
using MVC.Models.InDTO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2, 1")]
    public class UserController : ApiController
    {
        private readonly IUserService userService;

        private readonly IUnitOfWork unitOfWork;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
            this.userService = userService;

            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("api/user")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, UserSearch value)
        {
            try
            {
                var users = await userService.GetByName(value.FirstName, value.LastName);

                var usersPreviews = Mapper.Map<ICollection<User>, ICollection<UserPreviewDTO>>(users);

                return request.CreateResponse(HttpStatusCode.OK, usersPreviews);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/user/hrms")]
        public async Task<HttpResponseMessage> GetAllHRM(HttpRequestMessage request)
        {
            try
            {
                var hrs = await userService.GetAllHRMs();

                var hrsPreviews = Mapper.Map<ICollection<User>, ICollection<UserPreviewDTO>>(hrs);

                return request.CreateResponse(HttpStatusCode.OK, hrsPreviews);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/user/tech")]
        public async Task<HttpResponseMessage> GetAllTechs(HttpRequestMessage request)
        {
            try
            {
                var techs = await userService.GetAllTech();

                var techsPreviews = Mapper.Map<ICollection<User>, ICollection<UserPreviewDTO>>(techs);

                return request.CreateResponse(HttpStatusCode.OK, techsPreviews);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }
    }
}