using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BusinessLogic.Models;
using System;

namespace BusinessLogic.Services.Interfaces
{
    public interface IFileService : IDisposable
    {
        Task<HttpContent> GetResumeContentById(int id);

        Task AddResumeById(int id, HttpFileCollection files);

        Task<HttpContent> GetExcelByCandidates(int skip, int amount, CandidateSearchModel searchModel,
            CandidateSortModel sortModel);

        Task<HttpContent> GetExcelByVacancies(int skip, int amount, VacancySearchModel searchModel,
            VacancySortModel sortModel);

        string AddFile(string containerName, string newFileName, HttpFileCollection files);
    }
}
