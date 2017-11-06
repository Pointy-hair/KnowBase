using BusinessLogic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface IDictionaryRepository<T>: IReadRepository<T>, IFindRepository<T> where T: class 
    {
    }
}
