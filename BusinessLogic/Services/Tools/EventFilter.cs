using BusinessLogic.DBContext;
using BusinessLogic.Tools;
using System;

namespace BusinessLogic.Services.Tools
{
    public class EventFilter
    {
        public bool Filter(EventSearchOptions searchOption, Event someEvent)
        {
            if (searchOption.EventId != 0)
            {
                if (searchOption.EventId != someEvent.Event1)
                {
                    return false;
                }
            }
            if (searchOption.EventType != 0)
            {
                if (searchOption.EventType != someEvent.EventType)
                {
                    return false;
                }
            }
            if (searchOption.User != 0)
            {
                if (searchOption.User != someEvent.User)
                {
                    return false;
                }
            }
            if (searchOption.MinDate != DateTime.MinValue)
            {
                if (NotInDate(someEvent.Date, searchOption.MinDate, searchOption.MaxDate))
                {
                    return false;
                }
            }
            return true;
        }

        private bool NotInDate(DateTime date, DateTime minDate, DateTime maxDate)
        {
            return DateTime.Compare(date, maxDate) > 0 || DateTime.Compare(date, minDate) < 0;
        }
    }
}
