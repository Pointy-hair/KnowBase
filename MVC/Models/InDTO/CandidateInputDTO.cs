using System;
using System.Collections.Generic;

namespace MVC.Models.InDTO
{
    public class CandidateInputDTO
    {
        public List<CandidatePrevJobDTO> CandidatePrevJobs { get; set; }

        public TechSkillInDTO CandidatePrimarySkill { get; set; }

        public List<TechSkillInDTO> CandidateSecondarySkills { get; set; }

        public int? City { get; set; }

        public ContactsDTO Contact { get; set; }

        public DateTime? CustomerInterviewDate { get; set; }

        public DateTime? CustomerInterviewEndDate { get; set; }

        public bool? CustomerInterviewStatus { get; set; }

        public int? DesiredSalary { get; set; }

        public int? EngLevel { get; set; }

        public string FirstNameEng { get; set; }

        public string FirstNameRus { get; set; }

        public bool? GeneralInterviewStatus { get; set; }

        public int? Id { get; set; }

        public DateTime LastContactDate { get; set; }

        public string LastNameEng { get; set; }

        public string LastNameRus { get; set; }

        public string Picture { get; set; }

        public DateTime? PSExperience { get; set; }

        public DateTime? Reminder { get; set; }

        public int Status { get; set; }

        public bool? TechInterviewStatus { get; set; }

        public List<int> VacanciesIds { get; set; }
    }
}