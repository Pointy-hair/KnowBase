using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Models;
using MVC.Models;
using MVC.Models.InDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.AutoMapperProfiles
{
    public class DictionariesProfile : Profile
    {
        public DictionariesProfile()
        {
            CreateMap<CandidateStatus, CandidateStatusDTO>();

            CreateMap<City, CityDTO>();

            CreateMap<Contact, ContactsDTO>();

            CreateMap<EngLevel, EngLevelDTO>();

            CreateMap<EventId, EventDictionaryDTO>();

            CreateMap<EventType, EventDictionaryDTO>();

            CreateMap<GeneralSkill, GeneralSkillDTO>();

            CreateMap<TechSkill, TechSkillDTO>();

            CreateMap<CandidatePrimarySkill, TechSkillDTO>()
                .ForMember(skDTO => skDTO.Name, opt => opt.MapFrom(scSk => scSk.TechSkill1.Name))
                .ForMember(skDTO => skDTO.Picture, opt => opt.MapFrom(scSk => scSk.TechSkill1.Picture))
                .ForMember(skDTO => skDTO.Id, opt =>  opt.MapFrom(sk => sk.TechSkill));
            CreateMap<CandidateSecondarySkill, TechSkillDTO>()
                .ForMember(skDTO => skDTO.Name, opt => opt.MapFrom(scSk => scSk.TechSkill1.Name))
                .ForMember(skDTO => skDTO.Picture, opt => opt.MapFrom(scSk => scSk.TechSkill1.Picture))
                .ForMember(skDTO => skDTO.Id, opt => opt.MapFrom(sk => sk.TechSkill));

            CreateMap<TechSkillInDTO, CandidatePrimarySkill>()
                .ForMember(cps => cps.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));
            CreateMap<TechSkillInDTO, CandidateSecondarySkill>()
                .ForMember(css => css.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));

            CreateMap<TechSkillInDTO, VacancyPrimarySkill>()
                .ForMember(cps => cps.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));
            CreateMap<TechSkillInDTO, VacancySecondarySkill>()
                .ForMember(css => css.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));

            CreateMap<UserRole, UserRoleDTO>();

            CreateMap<VacancyStatus, VacancyStatusDTO>();

            CreateMap<TechSkillDTO, TechSkill>();

            CreateMap<ContactsDTO, Contact>();

            CreateMap<TechSkillInDTO, TechSkillModel>();
        }
    }
}