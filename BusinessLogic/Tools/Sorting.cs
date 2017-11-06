using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tools
{
    class SortingCandidates : IComparer<Candidate>
    {
        public int Compare(Candidate candidate1, Candidate candidate2)
        {
            if (candidate1.FirstNameEng != null || candidate2.FirstNameEng != null)
            {
                return candidate1.FirstNameEng.CompareTo(candidate2.FirstNameEng);
            }
            if (candidate1.FirstNameRus != null || candidate2.FirstNameRus != null)
            {
                return candidate1.FirstNameEng.CompareTo(candidate2.FirstNameEng);
            }
            return 0;
        }
    }

    class SortingVacancies : IComparer<Vacancy>
    {
        public int Compare(Vacancy x, Vacancy y)
        {
            throw new NotImplementedException();
        }
    }
    class SortingEvents : IComparer<Event>
    {
        public int Compare(Event x, Event y)
        {
            throw new NotImplementedException();
        }
    }
    class SortingNotifications : IComparer<Notification>
    {
        public int Compare(Notification x, Notification y)
        {
            throw new NotImplementedException();
        }
    }
}
