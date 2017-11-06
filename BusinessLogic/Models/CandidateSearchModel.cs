
namespace BusinessLogic.Models
{
    public class CandidateSearchModel
    {
        public string LastNameRus { get; set; }
        public string LastNameEng { get; set; }
        public int? PrimarySkill { get; set; }
        public int? HRM { get; set; }
        public int? City { get; set; }
        public int? Status { get; set; }

        public int? Level { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
    }
}