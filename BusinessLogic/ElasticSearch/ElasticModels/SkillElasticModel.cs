using Nest;

namespace BusinessLogic.ElasticSearch.ElasticModels
{
    public class SkillElasticModel
    {
        [Number]
        public int TechSkill { get; set; }
        [Text]
        public string Name { get; set; }
        [Number]
        public int Level { get; set; }
    }
}
