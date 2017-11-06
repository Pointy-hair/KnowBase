using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.IRepositories
{
    public interface IPagableRepository<T> where T: class
    {
        IEnumerable<T> PagingInFound(int skip, int amount, Expression<Func<T, int>> orderByProperty,
            IComparer<T> comp = null);

        IEnumerable<T> PagingDefaultOrder(int skip, int amount, Func<T, int> orderByProperty);
        IEnumerable<T> PagingDeafaultOrderByDecending(int skip, int amount, Func<T, int> orderByProperty);
    }
}
