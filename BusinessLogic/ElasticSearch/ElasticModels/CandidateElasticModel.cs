using System;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticModels
{
    public class CandidateElasticModel
    {
        [Text]
        public string FirstNameEng { get; set; }
        [Text]
        public string LastNameEng { get; set; }

        [Number]
        public int Id { get; set; }

        [Nested(IncludeInParent = true)]
        public StatusElasticModel Status { get; set; }

        [Nested(IncludeInParent = true)]
        public SkillElasticModel PrimarySkill { get; set; }

        [Text]
        public string Email { get; set; }

        [Text]
        public string Picture { get; set; }

        [Text]
        public string Phone { get; set; }

        [Text]
        public string Skype { get; set; }

        [Text]
        public string LastNameRus { get; set; }

        [Number]
        public int HRM { get; set; }

        [Number]
        public int City { get; set; }

        [Date]
        public DateTime LastContactDate { get; set; }

        [Date]
        public DateTime RemindDate { get; set; }
    }
}
