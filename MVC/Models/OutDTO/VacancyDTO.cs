using System;
using System.Collections.Generic;

namespace MVC.Models.OutDTO
{
    public class VacancyDTO
    {
        public ICollection<CandidatePreviewDTO> Candidates { get; set; }

        public string City { get; set; }

        public DateTime? CloseDate { get; set; }

        public string EngLevel { get; set; }

        public Nullable<int> Experience { get; set; }

        public UserPreviewDTO HRM { get; set; }

        public int Id { get; set; }

        public string Link { get; set; }

        public TechSkillDTO PrimarySkill { get; set; }

        public string ProjectName { get; set; }

        public DateTime? RequestDate { get; set; }

        public ICollection<TechSkillDTO> SecondarySkills { get; set; }

        public DateTime? StartDate { get; set; }

        public string Status { get; set; }

        public string VacancyName { get; set; }
    }
}