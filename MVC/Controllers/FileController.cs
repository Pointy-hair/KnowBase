using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BusinessLogic.Services.Interfaces;
using MVC.Models.InDTO;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class FileController : ApiController
    {
        private readonly IFileService fileService;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost]
        [Route("api/file/candidate/resume/add/{id}")]
        public async Task<HttpResponseMessage> AddFileToServer(HttpRequestMessage request, int id)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                await fileService.AddResumeById(id, httpRequest.Files);

                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/file/candidate/resume/get/{id}")]
        public async Task<HttpResponseMessage> GetResume(HttpRequestMessage request, int id)
        {
            try
            {
                var response = Request.CreateResponse();

                response.Headers.AcceptRanges.Add("bytes");

                response.StatusCode = HttpStatusCode.OK;

                var resume = await fileService.GetResumeContentById(id);

                response.Content = resume;

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(new { id }));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/file/candidate/excel/")]
        public async Task<HttpResponseMessage> GetExcelByCandidates(HttpRequestMessage request, CandidateSearchInputDTO value)
        {
            try
            {
                var response = Request.CreateResponse();

                response.Headers.AcceptRanges.Add("bytes");

                response.StatusCode = HttpStatusCode.OK;

                var excel = await fileService.GetExcelByCandidates(value.Skip, value.Amount,
                        value.SearchModel, value.SortModel);

                response.Content = excel;

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/file/vacancy/excel/")]
        public async Task<HttpResponseMessage> GetExcelByVacancies(HttpRequestMessage request, VacancySearchInputDTO value)
        {
            try
            {
                var response = Request.CreateResponse();

                response.Headers.AcceptRanges.Add("bytes");

                response.StatusCode = HttpStatusCode.OK;

                var excel = await fileService.GetExcelByVacancies(value.Skip, value.Amount,
                        value.SearchModel, value.SortModel);

                response.Content = excel;

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(value));

                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
