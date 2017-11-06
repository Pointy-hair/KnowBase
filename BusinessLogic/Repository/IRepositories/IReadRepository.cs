using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.IRepositories
{
    public interface IReadRepository<T> where T: class
    {
        T Read(int id);

        IQueryable<T> ReadAll();
    }
}
