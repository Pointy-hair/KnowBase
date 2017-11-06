using BusinessLogic.DBContext;
using System;
using BusinessLogic.ElasticSearch;
using System.Threading.Tasks;

namespace BusinessLogic.Unit_of_Work
{
    public interface IUnitOfWork : IDisposable
    {
        KnowBaseDBContext context { get; }

        ElasticSearchContext ElasticSearchContext { get; }

        int Save();

        void AttachToContext<T>(T obj) where T : class;

        void Detach<T>(T t) where T : class;

        Task DetachTechSkills();
    }
}
