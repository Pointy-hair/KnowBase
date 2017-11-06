using System;

namespace BusinessLogic.Tools
{
    public class EventSearchOptions
    {
        public int EventId { get; set; }

        public int EventType { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime MinDate { get; set; }

        public int User { get; set; }
    }
}
