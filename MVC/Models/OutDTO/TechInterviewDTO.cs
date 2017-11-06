using System;

namespace MVC.Models.OutDTO
{
    public class TechInterviewDTO
    {
        public string City { get; set; }

        public string Commentary { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        public string Feedback { get; set; }

        public UserPreviewDTO HRM { get; set; }

        public int Id { get; set; }

        public UserPreviewDTO Interviewer { get; set; }

        public bool Status { get; set; }

        public string TechSkill { get; set; }
    }
}