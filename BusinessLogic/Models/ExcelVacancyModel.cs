using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic.Models
{
    public class ExcelVacancyModel
    {
        public string ProjectName { get; set; }

        public string VacancyName { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? StartDate { get; set; }

        public string Status { get; set; }

        public string Link { get; set; }

        public string PrimarySkill { get; set; }

        public int PrimarySkillLevel { get; set; }

        public string EngLevel { get; set; }

        public int? Experience { get; set; }

        public string City { get; set; }

        public string HRM { get; set; }

        public string SecondarySkills { get; set; }

        public DateTime? CloseDate { get; set; }
    }
}