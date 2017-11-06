using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic.DBContext;

namespace BusinessLogic.Models
{
    public class CalendarEvent
    {
        public User HR { get; set; }
        public string TechEmail { get; set; }
        public string CandidateEmail { get; set; }
        public string Location { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TimeZone { get; set; }
    }
}