using System;

namespace MVC.Models.OutDTO
{
    public class EventDTO
    {
        public int CandidateId { get; set; }

        public DateTime Date { get; set; }

        public EventDictionaryDTO Event { get; set; }

        public EventDictionaryDTO EventType { get; set; }

        public int GeneralInterviewId { get; set; }

        public int Id { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string Property { get; set; }

        public int TechInterviewId { get; set; }

        public UserPreviewDTO User { get; set; }

        public int VacancyId { get; set; }
    }
}