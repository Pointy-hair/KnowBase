using System;

namespace MVC.Models.InDTO
{
    public class GeneralInterviewInDTO
    {
        public int Candidate { get; set; }

        public int? City { get; set; }

        public string Commentary { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Interviewer { get; set; }
    }
}