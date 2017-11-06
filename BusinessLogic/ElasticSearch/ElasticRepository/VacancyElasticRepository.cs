using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.ElasticSearch.ElasticModels;
using BusinessLogic.ElasticSearch.ElasticRepository.IRepository;
using BusinessLogic.Models;
using Microsoft.Practices.ObjectBuilder2;
using BusinessLogic.Unit_of_Work;
using Elasticsearch.Net;
using Nest;

namespace BusinessLogic.ElasticSearch.ElasticRepository
{
    public class VacancyElasticRepository: IVacancyElasticRepository
    {
        private readonly ElasticClient _client;
        private const string IndexName = "vacancies";
        private const string Type = "vacancyelasticmodel";

        public VacancyElasticRepository(IUnitOfWork unitOfWork)
        {
            _client = unitOfWork.ElasticSearchContext.ElasticClient;
            if (!_client.IndexExists(IndexName).Exists)
            {
                _client.CreateIndex(IndexName, index =>
                    index.Mappings(ms =>
                        ms.Map<VacancyElasticModel>(x => x.AutoMap())));
            }
        }

        public IBulkResponse BulkInsertCandidates(IEnumerable<VacancyElasticModel> candidates)
        {
            var bulk = new BulkDescriptor();
            candidates.ForEach(cand => bulk.Index<VacancyElasticModel>(i => i
                .Index(IndexName)
                .Id(cand.Id)
                .Document(cand)
            ));
            return _client.Bulk(bulk);
        }

        public IIndexResponse AddCandidate(VacancyElasticModel vacancy)
        {
            return _client.Index(vacancy, op => op
                .Id(vacancy.Id)
                .Index(IndexName)
            );
        }

        private static SortDescriptor<VacancyElasticModel> Sort(VacancySortModel sortModel)
        {
            var sort = new SortDescriptor<VacancyElasticModel>();

            if (sortModel == null)
            {
                return sort.Descending(p => p.Id);
            }
            if (sortModel.StartDate != null)
            {
                sort = sortModel.StartDate == true
                    ? sort.Ascending(p => p.StartDate)
                    : sort.Descending(p => p.StartDate);
            }
            else if (sortModel.RequestDate != null)
            {
                sort = sortModel.RequestDate == true
                    ? sort.Ascending(p => p.Id)
                    : sort.Descending(p => p.Id);
            }
            else if (sortModel.CloseDate != null)
            {
                sort = sortModel.CloseDate == true
                    ? sort.Ascending(p => p.CloseDate)
                    : sort.Descending(p => p.CloseDate);
            }
            else
            {
                sort = sort.Descending(p => p.Id);
            }
            return sort;
        }

        public IEnumerable<VacancyElasticModel> Search(int skip, int amount,
            VacancySearchModel searchModel, VacancySortModel sortModel)
        {

            var sort = Sort(sortModel);
            searchModel = searchModel ?? new VacancySearchModel();
            return _client.Search<VacancyElasticModel>(s =>
                s.Index(IndexName)
                    .Skip(skip)
                    .Take(amount)
                    .Query(q =>
                        {
                            QueryContainer queryContainer = null;

                            if (searchModel.City != 0)
                            {
                                queryContainer &= q.Nested(ns => ns
                                    .Path(p => p.City)
                                    .Query(qr => qr
                                        .Term(x => x.City.Id, searchModel.City)
                                    ));
                            }
                            if (searchModel.Status != 0)
                            {
                                queryContainer &= q.Nested(ns => ns
                                    .Path(p => p.Status)
                                    .Query(qr => qr
                                        .Term(x => x.Status.Id, searchModel.Status)
                                    ));
                            }

                            if (searchModel.PrimarySkill != null)
                            {
                                if (searchModel.PrimarySkill.Id != 0)
                                {
                                    queryContainer &= q.Nested(ns => ns
                                        .Path(p => p.PrimarySkill)
                                        .Query(qr => qr
                                            .Term(v => v.PrimarySkill.TechSkill, searchModel.PrimarySkill.Id)
                                        ));
                                }
                                if (searchModel.PrimarySkill.Level != 0)
                                {
                                    queryContainer &= q.Nested(ns => ns
                                        .Path(p => p.PrimarySkill)
                                        .Query(qr => qr
                                            .Term(v => v.PrimarySkill.Level, searchModel.PrimarySkill.Level)));
                                }
                            }
                            if (searchModel.RequestDate != DateTime.MinValue)
                            {
                                queryContainer &= q.Term(v => v.RequestDate, searchModel.RequestDate);
                            }
                            if (searchModel.StartDate != DateTime.MinValue)
                            {
                                queryContainer &= q.Term(v => v.StartDate, searchModel.StartDate);
                            }
                            //queryContainer &= q.MatchPhrase(fr => fr
                            //    .Field(z => z.ProjectName)
                            //    .Query(searchModel.ProjectName)
                            //    .Fuzziness(Fuzziness.Auto)
                            //    .FuzzyTranspositions()
                            //    .MaxExpansions(2)
                            //    .MinimumShouldMatch(2)
                            //    .PrefixLength(2)
                            //    .Operator(Operator.Or)
                            //    .Slop(2)
                            //    .Boost(2));
                            if (searchModel.ProjectName != null)
                            {
                                var projectNames = searchModel.ProjectName.Split(' ');
                                foreach (var projectName in projectNames)
                                {
                                    queryContainer &= q.MatchPhrase(mp =>
                                        mp.Field(f => f.ProjectName).Query(projectName)
                                            .Fuzziness(Fuzziness.EditDistance(2)));

                                }
                            }
                            if (searchModel.VacancyName != null)
                            {
                                var vacancyNames = searchModel.VacancyName.Split(' ');
                                foreach (var vacancyName in vacancyNames)
                                {
                                    queryContainer &= q.MatchPhrase(mp =>
                                        mp.Field(f => f.VacancyName).Query(vacancyName)
                                            .Fuzziness(Fuzziness.EditDistance(2)));

                                }
                            }
                            //queryContainer &= q.Fuzzy(x => x
                            //    .Field(z => z.ProjectName)
                            //    .Value(searchModel.ProjectName)
                            //    .Fuzziness(Fuzziness.Auto)
                            //);
                            //queryContainer &= q.Fuzzy(x => x
                            //        .Field(z => z.VacancyName)
                            //        .Value(searchModel.VacancyName)
                            //        .Fuzziness(Fuzziness.Auto)
                            //    );
                            return queryContainer;
                        }
                    )
        .Sort(p => p = sort)
            ).Documents;
        }

        public VacancyElasticModel SearchById(int id)
        {
            return _client.Search<VacancyElasticModel>(s => s
                .Index(IndexName)
                .Type(Type)
                .Query(q => q.Term(x => x.Id, id))).Documents.FirstOrDefault();
        }

        public IIndexResponse Update(int id, VacancyElasticModel model)
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
