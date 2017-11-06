using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DBContext
{
    public interface IInterview
    {
        int Id { get; set; }
        int Candidate { get; set; }
        int? City { get; set; }
        DateTime? Date { get; set; }
        int HRM { get; set; }
        int? Interviewer { get; set; }
        bool Status { get; set; }
        Candidate Candidate1 { get; set; }
        DateTime? EndDate { get; set; }
}
}
