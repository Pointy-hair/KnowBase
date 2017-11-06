using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic.Models
{
    public class ExcelCandidateModel
    {
        public string LastNameEng { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameRus { get; set; }

        public string FirstNameRus { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public string LinkedIn { get; set; }

        public string EngLevel { get; set; }

        public string City { get; set; }

        public string Status { get; set; }

        public string PrimarySkill { get; set; }

        public int PrimarySkillLevel { get; set; }

        public string SecondarySkills { get; set; }

        public DateTime? PSExperience { get; set; }

        public int? DesiredSalary { get; set; }

        public DateTime? LastContactDate { get; set; }

        public string HRM { get; set; }
    }
}