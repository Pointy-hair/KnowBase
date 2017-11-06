using System;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticModels
{
    public class VacancyElasticModel
    {
        [Number]
        public int Id { get; set; }

        [Text]
        public string ProjectName { get; set; }

        [Text]
        public string VacancyName { get; set; }

        [Date]
        public DateTime RequestDate { get; set; }

        [Date]
        public DateTime StartDate { get; set; }

        [Date]
        public DateTime CloseDate { get; set; }

        [Nested(IncludeInParent = true)]
        public CityElasticModel City { get; set; }

        [Nested(IncludeInParent = true)]
        public StatusElasticModel Status { get; set; }

        [Nested(IncludeInParent = true)]
        public SkillElasticModel PrimarySkill { get; set; }

    }
}
