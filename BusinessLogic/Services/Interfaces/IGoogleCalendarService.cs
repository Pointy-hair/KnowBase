using BusinessLogic.Models;
using System;

namespace BusinessLogic.Services.Interfaces
{
    public interface IGoogleCalendarService
    {
        CalendarEvent DefaultEvent(int interviewId, string type, int? candidateId);

        string AddEvent(CalendarEvent calendarEvent);
    }
}
