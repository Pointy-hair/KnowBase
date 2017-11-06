using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Repository.IRepositories
{
    public interface IFindRepository<T> where T: class
    {
        IQueryable<T> Find(Expression<Func<T, bool>> filter);
    }
}
