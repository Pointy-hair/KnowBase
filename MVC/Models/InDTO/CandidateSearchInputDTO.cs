using BusinessLogic.Models;
using BusinessLogic.Tools;

namespace MVC.Models.InDTO
{
    public class CandidateSearchInputDTO
    {
        public int Amount { get; set; }

        public int Skip { get; set; }

        public CandidateSearchModel SearchModel { get; set; }

        public CandidateSortModel SortModel { get; set; }
    }
}