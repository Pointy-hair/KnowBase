using BusinessLogic.DBContext;
using BusinessLogic.Unit_of_Work;
using System;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Repository.UnityRepositories;
using System.Collections.Generic;

namespace BusinessLogic.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(IUnitOfWork context) : base(context)
        {
        }

        public IEnumerable<Event> AddRange(ICollection<Event> events)
        {
            var result = dbSet.AddRange(events);

            return result;
        }
    }
}
