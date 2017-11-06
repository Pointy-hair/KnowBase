using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class VacancySearchModel
    {
        public string ProjectName { get; set; }

        public string VacancyName { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime StartDate { get; set; }

        public int City { get; set; }

        public int Status { get; set; }

        public TechSkillModel PrimarySkill { get; set; }
    }
}
