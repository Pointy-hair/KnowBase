using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.IRepositories
{
    public interface ICreateRepository<T> where T : class
    {
        T Create(T t);
    }
}
