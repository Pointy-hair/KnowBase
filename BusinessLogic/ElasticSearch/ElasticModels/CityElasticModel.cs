using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ElasticSearch.ElasticModels
{
    public class CityElasticModel
    {
        [Number]
        public int Id { get; set; }

        [Text]
        public string Name { get; set; }
    }
}
