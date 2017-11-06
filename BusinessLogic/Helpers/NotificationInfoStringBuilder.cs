using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public static class NotificationInfoStringBuilder
    {
        public static string GetSnackString(Notification notification)
        {
            var result = string.Empty;

            if (notification.Type == NotificationTypes.Assignment)
            {
                result = "You have new assignment.";
            }
            else if (notification.Type == NotificationTypes.Interview)
            {
                result = "You have new interview.";
            }
            else if (notification.Type == NotificationTypes.Reminder)
            {
                result = "You have new reminder";
            }
            else
            {
                result = "You have new update.";
            }

            return result;
        }

        public static string GetNotificationTitle(Notification notification)
        {
            var result = string.Empty;

            var e = notification.Events.First();

            if (e.Candidate.HasValue)
            {
                if (e.EventType == EventTypes.Edit)
                {
                    result += "Candidate " + e.Candidate1.FirstNameEng + " " + e.Candidate1.LastNameEng + " ";

                    result += "was edited by " + e.User1.Name;
                }
            }
            else if (e.GeneralInterview.HasValue)
            {
                if (e.Event1 == EventIds.Feedback)
                {
                    result += "User " + e.GeneralInterview1.User1.Name + 
                        " left feedback on interview with " + e.GeneralInterview1.Candidate1.FirstNameEng + 
                        " " + e.GeneralInterview1.Candidate1.LastNameEng;
                }
                else
                {
                    result += "User " + e.GeneralInterview1.User.Name + " assigned interview to candidate " +
                        e.GeneralInterview1.Candidate1.FirstNameEng + " " + e.GeneralInterview1.Candidate1.LastNameEng;
                }
            }
            else if (e.TechInterview.HasValue)
            {
                if (e.Event1 == EventIds.Feedback)
                {
                    result += "User " + e.TechInterview1.User1.Name +
                        " left feedback on interview with " + e.TechInterview1.Candidate1.FirstNameEng +
                        " " + e.TechInterview1.Candidate1.LastNameEng;
                }
                else
                {
                    result += "User " + e.TechInterview1.User.Name + " assigned interview to candidate " +
                        e.TechInterview1.Candidate1.FirstNameEng + " " + e.TechInterview1.Candidate1.LastNameEng;
                }
            }
            else if (e.Vacancy.HasValue)
            {
                if (e.EventType == EventTypes.Edit)
                {
                    result += "Position " + e.Vacancy1.ProjectName + ", " + e.Vacancy1.VacancyName + " ";

                    result += "was edited by " + e.User1.Name;
                }
            }

            if (result == string.Empty)
            {
                result = GetSnackString(notification);
            }

            return result;
        }
    }
}
