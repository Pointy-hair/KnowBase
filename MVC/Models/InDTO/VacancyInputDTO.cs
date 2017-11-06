using System;
using System.Collections.Generic;

namespace MVC.Models.InDTO
{
    public class VacancyInputDTO
    {
        public ICollection<int> Candidates { get; set; }

        public int City { get; set; }

        public DateTime? CloseDate { get; set; }

        public int EngLevel { get; set; }

        public int Experience { get; set; }

        public int Id { get; set; }

        public string Link { get; set; }

        public TechSkillInDTO PrimarySkill { get; set; }

        public string ProjectName { get; set; }

        public DateTime? RequestDate { get; set; }

        public ICollection<TechSkillInDTO> SecondarySkills { get; set; }

        public DateTime? StartDate { get; set; }

        public int Status { get; set; }

        public string VacancyName { get; set; }
    }
}