using System.Web.Http;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using MVC.Models;
using MVC.Models.InDTO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System;
using BusinessLogic.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "3, 2")]
    public class MailController : ApiController
    {
        private readonly IMailService mailService;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("api/mail/interview/add/candidate")]
        public async Task<HttpResponseMessage> GetFormByAddInterviewToCandidate(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.AddInterviewForCandidate(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/interview/add/interviewer")]
        public async Task<HttpResponseMessage> GetFormByAddInterviewToInterviewer(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.AddInterviewForInterviewer(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/interview/changeDate/candidate")]
        public async Task<HttpResponseMessage> GetFormByChangeInterviewToCandidate(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.ChangeInterviewDateForCandidate(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/interview/changeDate/interviewer")]
        public async Task<HttpResponseMessage> GetFormByChangeInterviewToInterviewer(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.ChangeInterviewDateForInterviewer(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/interview/cancel/candidate")]
        public async Task<HttpResponseMessage> GetFormByCancelInterviewToCandidate(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.CancelInterviewForCandidate(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/interview/cancel/interviewer")]
        public async Task<HttpResponseMessage> GetFormByCancelInterviewToInterviewer(HttpRequestMessage request, [FromBody]MailRequest value)
        {
            try
            {
                var mail = await mailService.CancelInterviewForInterviewer(value, 1);

                return request.CreateResponse(HttpStatusCode.OK, mail);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/mail/send")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, [FromBody]Mail value)
        {
            try
            {
                await mailService.Send(value);

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
