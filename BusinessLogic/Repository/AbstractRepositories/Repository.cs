using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Repository.AbstractRepositories
{
    public abstract class Repository<T>: IReadRepository<T>, ICreateRepository<T>, IDeleteRepository,
        IFindRepository<T>, IPagableRepository<T> where T: class 
    {
        protected readonly KnowBaseDBContext context;

        protected readonly DbSet<T> dbSet;

        protected IQueryable<T> SequenceForPaging;

        protected Repository(IUnitOfWork unitOfWork)
        {
            this.context = unitOfWork.context;

            dbSet = context.Set<T>();

            SequenceForPaging = dbSet;
        }

        public T Read(int id)
        {
            try
            {
                return dbSet.Find(id); // пока не написали ексепшены
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IQueryable<T> ReadAll()
        {
            try
            {
                // tbd
                return dbSet;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T Create(T t)
        {
            try
            {
                var newT = dbSet.Add(t);

                return newT;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<T> PagingInFound(int skip, int amount, Expression<Func<T, int>> orderByProperty, IComparer<T> comp = null)
        {
            try
            {
                return comp != null
                    ? SequenceForPaging.AsEnumerable().OrderBy(a => a, comp).Skip(skip).Take(amount)
                    : SequenceForPaging.OrderByDescending(orderByProperty.Compile()).Skip(skip).Take(amount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<T> PagingDefaultOrder(int skip, int amount, Func<T, int> orderByProperty)
        {
            return dbSet.OrderBy(orderByProperty).Skip(skip).Take(amount);
        }

        public IEnumerable<T> PagingDeafaultOrderByDecending(int skip, int amount, Func<T, int> orderByProperty)
        {
            return dbSet.OrderByDescending(orderByProperty).Skip(skip).Take(amount);
        }


        public IQueryable<T> Find(Expression<Func<T, bool>> filter)
        {
            try
            {
                SequenceForPaging = dbSet.Where(filter.Compile()).AsQueryable();

                return SequenceForPaging;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var obj = dbSet.Find(id);

                dbSet.Remove(obj);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
