using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic.Models
{
    public class MailRequest
    {
        public int Candidate { get; set; }

        public int Interviewer { get; set; }

        public DateTime Date { get; set; }

        public string Skill { get; set; }
    }
}