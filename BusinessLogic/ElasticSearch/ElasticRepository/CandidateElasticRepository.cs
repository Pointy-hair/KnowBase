using System.Collections.Generic;
using System.Linq;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticRepository.IRepository;
using BusinessLogic.Models;
using BusinessLogic.Unit_of_Work;
using Elasticsearch.Net;
using Microsoft.Practices.ObjectBuilder2;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticRepository
{
    public class CandidateElasticRepository: ICandidateElasticRepository
    {
        private readonly ElasticClient _client;
        private const string IndexName = "candidates";
        private const string Type = "candidateelasticmodel";

        public CandidateElasticRepository(IUnitOfWork unitOfWork)
        {
            _client = unitOfWork.ElasticSearchContext.ElasticClient;
            if (!_client.IndexExists(IndexName).Exists)
            {
                _client.CreateIndex(IndexName, index =>
                    index.Mappings(ms =>
                        ms.Map<CandidateElasticModel>(x => x.AutoMap())));
            }
        }

        public IBulkResponse BulkInsertCandidates(IEnumerable<CandidateElasticModel> candidates)
        {
            var bulk = new BulkDescriptor();
            candidates.ForEach(cand => bulk.Index<CandidateElasticModel>(i => i
            .Index(IndexName)
            .Id(cand.Id)
            .Document(cand)
            ));
            return _client.Bulk(bulk);
        }

        public IIndexResponse AddCandidate(CandidateElasticModel candidate)
        {
            return _client.Index(candidate, op => op
                .Id(candidate.Id)
                .Index(IndexName)
            );
        }

        private static SortDescriptor<CandidateElasticModel> Sort(CandidateSortModel sortModel)
        {
            var sort = new SortDescriptor<CandidateElasticModel>();

            if (sortModel == null)
            {
                return sort.Descending(p => p.Id);
            }

            if (sortModel.LastContactDate != null)
            {
                sort = sortModel.LastContactDate == true
                    ? sort.Ascending(p => p.LastContactDate)
                    : sort.Descending(p => p.LastContactDate);
            }
            else if (sortModel.CreationDate != null)
            {
                sort = sortModel.CreationDate == true
                    ? sort.Ascending(p => p.Id)
                    : sort.Descending(p => p.Id);
            }
            else if (sortModel.RemindDate != null)
            {
                sort = sortModel.RemindDate == true
                    ? sort.Ascending(p => p.RemindDate)
                    : sort.Descending(p => p.RemindDate);
            }
            else
            {
                sort = sort.Descending(p => p.Id);
            }
            return sort;
        }

        public IEnumerable<CandidateElasticModel> Search(int skip, int amount,
            CandidateSearchModel searchModel, CandidateSortModel sortModel)
        {
            var sort = Sort(sortModel);
            searchModel = searchModel ?? new CandidateSearchModel();
            return _client.Search<CandidateElasticModel>(s =>
                s.Index(IndexName)
                    .Skip(skip)
                    .Take(amount)
                    .Query(q =>
                        q.Nested(ns => ns
                            .Path(p => p.PrimarySkill)
                            .Query(qr => qr
                                .Term(x => x.PrimarySkill.TechSkill, searchModel.PrimarySkill)
                            ))&&
                        q.Nested(ns => ns
                            .Path(p => p.PrimarySkill)
                            .Query(qr => qr
                                .Term(x => x.PrimarySkill.Level, searchModel.Level)
                            )) &&
                        q.Term(x => x.HRM, searchModel.HRM) &&
                        q.Term(x => x.City, searchModel.City) &&
                        q.Term(x => x.Status.Id, searchModel.Status) &&
                        //q.Match(m => m
                        //    .Field(x => x.Phone)
                        //    .Query(searchModel.Phone)
                        //    .Fuzziness(Fuzziness.Auto)
                        //) &&
                        q.Fuzzy(x => x
                        .Field(z => z.LastNameEng)
                       // .Boost(1)
                        .Value(searchModel.LastNameEng)
                        .Fuzziness(Fuzziness.Auto)
                        ) &&
                        q.Fuzzy(x => x
                            .Field(z => z.Skype)
                            .Value(searchModel.Skype)
                            .Fuzziness(Fuzziness.Auto)
                         ) &&
                        q.Fuzzy(x => x
                            .Field(z => z.Email)
                            .Value(searchModel.Email)
                            .Fuzziness(Fuzziness.Auto)
                        ) &&
                        q.Fuzzy(x => x
                            .Field(z => z.Phone)
                            .Value(searchModel.Phone)
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                    .Sort(p =>p = sort)
            ).Documents;
        }

        public CandidateElasticModel SearchById(int id)
        {
            return _client.Search<CandidateElasticModel>(s => s
                .Index(IndexName)
                .Type(Type)
                .Query(q => q.Term(x => x.Id, id))).Documents.FirstOrDefault();
        }
        public IIndexResponse Update(int id, CandidateElasticModel model)
        {
            return _client.Index(model, i => i
                .Index(IndexName)
                .Type(Type)
                .Id(id)
                .Refresh(Refresh.True)
            );

        }
    }
}
