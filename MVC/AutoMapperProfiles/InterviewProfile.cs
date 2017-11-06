using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BusinessLogic.DBContext;
using MVC.Models;
using MVC.Models.InDTO;
using MVC.Models.OutDTO;

namespace MVC.AutoMapperProfiles
{
    public class InterviewProfile : Profile
    {
        public InterviewProfile()
        {
            CreateMap<TechInterviewInDTO, TechInterview>();
            CreateMap<InterviewUpdateDTO, TechInterview>();
            CreateMap<TechInterview, TechInterviewDTO>()
                .ForMember(dto => dto.TechSkill, opt => opt.MapFrom(tech => tech.TechSkill1.Name))
                .ForMember(dto => dto.HRM, opt => opt.MapFrom(tech => Mapper.Map<User, UserPreviewDTO>(tech.User)))
                .ForMember(dto => dto.Interviewer, opt => opt.MapFrom(tech => Mapper.Map<User, UserPreviewDTO>(tech.User1)))
                .ForMember(dto => dto.City, opt => opt.MapFrom(tech => tech.City1.Name));

            CreateMap<GeneralInterview, GeneralInterviewDTO>()
                .ForMember(dto => dto.EngLevel, opt => opt.MapFrom(gen => gen.EngLevel1.Name))
                .ForMember(dto => dto.City, opt => opt.MapFrom(gen => gen.City1.Name))
                .ForMember(dto => dto.HRM, opt => opt.MapFrom(gen => Mapper.Map<User, UserPreviewDTO>(gen.User)))
                .ForMember(dto => dto.Interviewer,
                    opt => opt.MapFrom(gen => Mapper.Map<User, UserPreviewDTO>(gen.User1)));

            CreateMap<InterviewUpdateDTO, GeneralInterview>();

            CreateMap<GeneralInterviewFeedbackInDTO, GeneralInterview>();

            CreateMap<TechInterviewFeedbackInDTO, TechInterview>();

            CreateMap<GeneralInterviewInDTO, GeneralInterview>();
        }
    }
}