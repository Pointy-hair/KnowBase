using System;

namespace MVC.Models.OutDTO
{
    public class VacancyPreviewDTO
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string VacancyName { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? StartDate { get; set; }

        public string City { get; set; }

        public TechSkillDTO PrimarySkill { get; set; }

        public string Status { get; set; }

    }
}