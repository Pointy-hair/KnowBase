using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface IEventRepository : IFindRepository<Event>, IPagableRepository<Event>, ICreateRepository<Event>
    {
        IEnumerable<Event> AddRange(ICollection<Event> events);
    }
}
