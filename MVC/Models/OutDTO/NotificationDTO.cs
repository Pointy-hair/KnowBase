using System;

namespace MVC.Models.OutDTO
{
    public class NotificationDTO
    {
        public bool Active { get; set; }

        public CandidatePreviewDTO Cadidate { get; set; }

        public DateTime Date { get; set; }

        public GeneralInterviewDTO GeneralInterview { get; set; }

        public int Id { get; set; }

        public string NotificationType { get; set; }

        public TechInterviewDTO TechInterview { get; set; }

        public string Title { get; set; }

        public VacancyPreviewDTO Vacancy { get; set; }
    }
}