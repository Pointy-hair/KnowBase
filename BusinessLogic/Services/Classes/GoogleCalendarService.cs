using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Practices.ObjectBuilder2;
using Event = Google.Apis.Calendar.v3.Data.Event;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Classes
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };

        private const string ApplicationName = "KnowBase";

        private const string CalendarId = "3qpiqs5430id91hm1b1r2fanks@group.calendar.google.com";

        private const string CalendarName = "KnowBaseCalendar";

        private UserCredential credential;

        private CalendarService service;

        private readonly IUserRepository userRepository;

        private readonly ITechInterviewRepository techInterviewRepository;

        private readonly IGeneralInterviewRepository generalInterviewRepository;

        private readonly ICandidateRepository candidateRepository;

        public GoogleCalendarService(IUserRepository userRepository, 
            ITechInterviewRepository techInterviewRepository,
            IGeneralInterviewRepository generalInterviewRepository, 
            ICandidateRepository candidateRepository)
        {
            this.userRepository = userRepository;

            this.techInterviewRepository = techInterviewRepository;

            this.generalInterviewRepository = generalInterviewRepository;

            this.candidateRepository = candidateRepository;

            Initialize();
        }

        public string AddEvent(CalendarEvent calendarEvent)
        {

            var attendies = new List<EventAttendee>
            {
                new EventAttendee{Email = calendarEvent.TechEmail}
            };

            if (calendarEvent.CandidateEmail != null)
            {
                attendies.Add(new EventAttendee { Email = calendarEvent.CandidateEmail });
            }

            var newEvent = new Event
            {
                Summary = calendarEvent.Summary,

                Location = calendarEvent.Location,

                Description = calendarEvent.Description,

                Start = new EventDateTime()
                {
                    DateTime = calendarEvent.StartTime,

                    TimeZone = calendarEvent.TimeZone
                },

                End = new EventDateTime()
                {
                    DateTime = calendarEvent.EndTime,

                    TimeZone = calendarEvent.TimeZone
                },

                Recurrence = new[] { "RRULE:FREQ=DAILY;COUNT=1" },

                Attendees = attendies,

                Reminders = new Event.RemindersData()
                {
                    UseDefault = true
                },

                GuestsCanInviteOthers = false,

                GuestsCanSeeOtherGuests = false
            };

            /* if (calendarEvent.HR.CalendarId == null)
             {
                 calendarEvent.HR.CalendarId = AddCalendarForUser(calendarEvent.HR);
                 unitOfWork.Save();
             }*/
            //AddCalendar();
            var request = service.Events.Insert(newEvent, CalendarId);

            request.SendNotifications = true;

            var createdEvent = request.Execute();

            return createdEvent.HtmlLink;
        }

        public CalendarEvent DefaultEvent(int interviewId, string type, int? candidateId)
        {
            CalendarEvent calendarEvent;

            if (type == "tech")
            {
                var interview = techInterviewRepository.Read(interviewId);

                calendarEvent = new CalendarEvent
                {
                    HR = interview.User,

                    CandidateEmail = interview.Candidate1.Contact.Email,

                    Summary = "JobInterview",

                    Description = "Job Technical Interview for Exadel.",

                    TechEmail = interview.User1.Email,

                    StartTime = interview.Date,

                    EndTime = interview.EndDate
                };
            }
            else if (type == "general")
            {
                var interview = generalInterviewRepository.Read(interviewId);

                calendarEvent = new CalendarEvent
                {
                    HR = interview.User,

                    CandidateEmail = interview.Candidate1.Contact.Email,

                    Summary = "JobInterview",

                    Description = "Job General Interview for Exadel.",

                    TechEmail = interview.User1.Email,

                    StartTime = interview.Date,

                    EndTime = interview.EndDate
                };
            }
            else
            {
                candidateId = candidateId ?? throw new Exception("Not found candidateId");

                var candidate = candidateRepository.Read(candidateId.Value);

                calendarEvent = new CalendarEvent
                {
                    HR = candidate.User1,

                    CandidateEmail = candidate.Contact.Email,

                    Summary = "JobInterview",

                    Description = "Job Customer Interview for Exadel.",

                    StartTime = candidate.CustomerInterviewDate,

                    EndTime = candidate.CustomerInterviewEndDate
                };
            }

            return calendarEvent;
        }

        private void Initialize()
        {
            var secret = HttpContext.Current.Server.MapPath("~/client_secret.json");

            using (var stream = new FileStream(secret, FileMode.Open, FileAccess.Read))
            {
                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,

                    Scopes,

                    "user",

                    CancellationToken.None,

                    new FileDataStore(credPath, true)
                ).Result;
            }

            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,

                ApplicationName = ApplicationName,
            });
        }

       /* private string AddCalendarForUser(User user)
        {

            string calendarName = $"{user.Name}KnowBaseCalendar";
            Calendar calendar = new Calendar
            {
                Summary = calendarName,
                TimeZone = "UTC"
            };
            Calendar createdCalendar = service.Calendars.Insert(calendar).Execute();
           // user.CalendarId = createdCalendar.Id;
           AddWriterForCalendar(user.Email, createdCalendar.Id);
            return createdCalendar.Id;
        }*/

        private void AddCalendar()
        {
            var calendar = new Calendar
            {
                Summary = CalendarName,

                TimeZone = "UTC"
            };

            var createdCalendar = service.Calendars.Insert(calendar).Execute();

            var id = createdCalendar.Id;
        }

        /* private void AddWriterForCalendar(string email, string calendarName)
         {
             AclRule rule = new AclRule
             {
                 Scope = new AclRule.ScopeData {Value = email, Type = "user"},
                 Role = "writer"
             };

             //Insert new access rule
            AclRule createdRule = service.Acl.Insert(rule, calendarName).Execute();

         }*/
    }
}
