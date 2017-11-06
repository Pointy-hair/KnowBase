using System;

namespace MVC.Models.InDTO
{
    public class InterviewUpdateDTO
    {
        public int? Candidate { get; set; }

        public string Commentary { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Id { get; set; }

        public int? Interviewer { get; set; }
    }
}