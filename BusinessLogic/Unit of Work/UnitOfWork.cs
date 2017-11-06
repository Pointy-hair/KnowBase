using System;
using BusinessLogic.DBContext;
using System.Data.Entity;
using BusinessLogic.ElasticSearch;
using System.Threading.Tasks;

namespace BusinessLogic.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        public KnowBaseDBContext context { get; set; } = new KnowBaseDBContext();

        public ElasticSearchContext ElasticSearchContext { get; set; } = new ElasticSearchContext();

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (context != null)
            {
                context.Dispose();
                context = null;
            }

            if (ElasticSearchContext != null)
            {
                ElasticSearchContext = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public int Save()
        {
            var res = context.SaveChanges();

            return res;
        }

        public void AttachToContext<T>(T obj) where T : class
        {
            var dbset = context.Set<T>();

            dbset.Attach(obj);
        }

        public void Detach<T>(T t) where T: class
        {
            try
            {
                context.Entry(t).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DetachTechSkills()
        {
            try
            {
                await context.TechSkills.ForEachAsync(ts => context.Entry(ts).State = EntityState.Detached);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
