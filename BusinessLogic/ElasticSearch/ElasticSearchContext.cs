using System;
using Nest;

namespace BusinessLogic.ElasticSearch
{
    public class ElasticSearchContext
    {
        public ElasticClient ElasticClient { get;}
        public string DefaultIndexName { get; } = "knowbase";
        public ElasticSearchContext()
        {
            var node = new Uri("http://bitnami-elasticsearch-80d3-ip.northeurope.cloudapp.azure.com/elasticsearch/");

            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(DefaultIndexName);
            settings.BasicAuthentication("user", "iAbj22zJ");

            ElasticClient = new ElasticClient(settings);
        }
    }
}