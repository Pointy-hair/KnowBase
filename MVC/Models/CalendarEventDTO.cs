using System;

namespace MVC.Models
{
    public class CalendarEventDTO
    {
        public string CandidateEmail { get; set; }

        public string Description { get; set; }

        public DateTime? EndTime { get; set; }

        public string Location { get; set; }

        public DateTime? StartTime { get; set; }

        public string Summary { get; set; }

        public string TechEmail { get; set; }

        public string TimeZone { get; set; }
    }
}