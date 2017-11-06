using System;

namespace MVC.Models.InDTO
{
    public class GeneralInterviewFeedbackInDTO
    {
        public int? EngLevel { get; set; }

        public string Feedback { get; set; }

        public int Id { get; set; }

        public string Interests { get; set; }

        public string JobChangeReason { get; set; }

        public bool? ReadyForBusinessTrips { get; set; }

        public int? SalaryExpectations { get; set; }

        public DateTime? WhenCanStart { get; set; }

        public int NotificationId { get; set; }
    }
}