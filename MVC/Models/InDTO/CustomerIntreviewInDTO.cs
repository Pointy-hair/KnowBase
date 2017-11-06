using System;

namespace MVC.Models.InDTO
{
    public class CustomerIntreviewInDTO
    {
        public int Candidate { get; set; }

        public string Commentary { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }
    }
}