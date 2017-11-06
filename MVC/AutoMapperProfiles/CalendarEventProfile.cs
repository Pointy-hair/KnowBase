using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Models;
using MVC.Models;
using MVC.Models.OutDTO;

namespace MVC.AutoMapperProfiles
{
    public class CalendarEventProfile: Profile
    {
        public CalendarEventProfile()
        {
            CreateMap<CalendarEvent, CalendarEventDTO>();

            CreateMap<CalendarEventDTO, CalendarEvent>();
        }
    }
}