using System;

namespace MVC.Models.OutDTO
{
    public class GeneralInterviewDTO
    {
        public string City { get; set; }

        public string Commentary { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? EndDate { get; set; }

        public string EngLevel { get; set; }

        public string Feedback { get; set; }

        public UserPreviewDTO HRM { get; set; }

        public UserPreviewDTO Interviewer { get; set; }

        public bool Status { get; set; }

        public string JobChangeReason { get; set; }

        public DateTime? WhenCanStart { get; set; }

        public bool? ReadyForBusinessTrips { get; set; }

        public string Interests { get; set; }

        public int? SalaryExpectations { get; set; }
    }
}