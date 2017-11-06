using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tools
{
    public static class VacancyStatuses
    {
        public const int OnHold = 1;

        public const int Active = 2;

        public const int CVProvided = 3;

        public const int WaitingForInterviewWithCustomer = 4;

        public const int InterviewWithCustomer = 5;

        public const int CandidateDeclined = 6;

        public const int CadidateApproved = 7;

        public const int Closed = 8;

        public const int Cancelled = 9;
    }
}
