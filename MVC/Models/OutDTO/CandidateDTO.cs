using MVC.Models.OutDTO;
using System;
using System.Collections.Generic;

namespace MVC.Models.OutDTO
{
    public class CandidateDTO
    {
        public string City { get; set; }

        public List<CandidatePrevJobDTO> CandidatePrevJobsContacts { get; set; }

        public TechSkillDTO CandidatePrimarySkill { get; set; }

        public List<TechSkillDTO> CandidateSecondarySkills { get; set; }

        public ContactsDTO Contact { get; set; }

        public DateTime? CustomerInterviewDate { get; set; }

        public bool? CustomerInterviewStatus { get; set; }

        public int Id { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string FirstNameRus { get; set; }

        public string LastNameRus { get; set; }

        public int? DesiredSalary { get; set; }

        public string EngLevel { get; set; }

        public bool? GeneralInterviewStatus { get; set; }

        public UserPreviewDTO HRM { get; set; }

        public DateTime? LastContactDate { get; set; }

        public UserPreviewDTO LastModifier { get; set; }

        public string Picture { get; set; }

        public DateTime? PSExperience { get; set; }

        public DateTime? Reminder { get; set; }

        public string Status { get; set; }

        public bool? TechInterviewStatus { get; set; }

        public List<VacancyPreviewDTO> Vacancies { get; set; }
    }
}