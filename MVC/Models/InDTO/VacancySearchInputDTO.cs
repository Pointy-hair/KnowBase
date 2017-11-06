
using BusinessLogic.Models;

namespace MVC.Models.InDTO
{
    public class VacancySearchInputDTO
    {
        public int Amount { get; set; }

        public int Skip { get; set; }

        public VacancySearchModel SearchModel { get; set; }

        public VacancySortModel SortModel { get; set; }
    }
}