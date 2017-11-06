using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class ReturnWrapper<T> where T : class
    {
        public Dictionary<int, List<Event>> Events { get; set; }

        public ICollection<T> Entities { get; set; }
    }
}
