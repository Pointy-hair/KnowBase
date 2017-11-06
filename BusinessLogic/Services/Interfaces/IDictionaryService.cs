using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IDictionaryService<T> : IDisposable where T: class 
    {
        Task<ICollection<T>> GetAll();
    }
}
