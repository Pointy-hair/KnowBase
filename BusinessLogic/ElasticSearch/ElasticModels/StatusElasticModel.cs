using Nest;

namespace BusinessLogic.ElasticSearch.ElasticModels
{
    public class StatusElasticModel
    {
        [Number]
        public int Id { get; set; }
        [Text]
        public string Name { get; set; }
    }
}
