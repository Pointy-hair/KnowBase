using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.UnityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Repository.DictionaryRepositories
{
    public class DictionaryRepository<T> : Repository<T>, IDictionaryRepository<T> where T : class
    {
        public DictionaryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
