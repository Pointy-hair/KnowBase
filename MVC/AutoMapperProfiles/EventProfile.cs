using AutoMapper;
using BusinessLogic.DBContext;
using MVC.Models;
using MVC.Models.OutDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.AutoMapperProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventId, EventDictionaryDTO>();
            CreateMap<EventType, EventDictionaryDTO>();

            CreateMap<Event, EventDTO>()
                .ForMember(edto => edto.Event, opt => opt.MapFrom(edb => Mapper.Map<EventId, EventDictionaryDTO>(edb.EventId)))
                .ForMember(edto => edto.EventType, opt => opt.MapFrom(edb => Mapper.Map<EventType, EventDictionaryDTO>(edb.EventType1)))
                .ForMember(edto => edto.User, opt => opt.MapFrom(edb => Mapper.Map<User, UserPreviewDTO>(edb.User1)))
                .ForMember(edto => edto.CandidateId, opt => opt.MapFrom(edb => edb.Candidate ?? 0))
                .ForMember(edto => edto.VacancyId, opt => opt.MapFrom(edb => edb.Vacancy ?? 0))
                .ForMember(edto => edto.GeneralInterviewId, opt => opt.MapFrom(edb => edb.GeneralInterview ?? 0))
                .ForMember(edto => edto.TechInterviewId, opt => opt.MapFrom(edb => edb.TechInterview ?? 0));
        }
    }
}