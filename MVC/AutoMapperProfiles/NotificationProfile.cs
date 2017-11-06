using AutoMapper;
using BusinessLogic.DBContext;
using MVC.Models.OutDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.AutoMapperProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDTO>()
                .ForMember(ndto => ndto.Active, opt => opt.MapFrom(n => n.Active))
                .ForMember(ndto => ndto.Cadidate, opt => opt.MapFrom(n => n.Events.FirstOrDefault().Candidate1))
                .ForMember(ndto => ndto.GeneralInterview, opt => opt.MapFrom(n => n.Events.FirstOrDefault().GeneralInterview1))
                .ForMember(ndto => ndto.TechInterview, opt => opt.MapFrom(n => n.Events.FirstOrDefault().TechInterview1))
                .ForMember(ndto => ndto.Vacancy, opt => opt.MapFrom(n => n.Events.FirstOrDefault().Vacancy1))
                .ForMember(ndto => ndto.NotificationType, opt => opt.MapFrom(n => n.NotificationType.Name));
        }
    }
}