using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.Models;
using MVC.Models;
using MVC.Models.InDTO;
using MVC.Models.OutDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.AutoMapperProfiles
{
    public class VacancyProfile : Profile 
    {
        public VacancyProfile()
        {
            CreateMap<VacancyPrimarySkill, TechSkillDTO>()
                .ForMember(tsdto => tsdto.Id, opt => opt.MapFrom(vps => vps.TechSkill1.Id))
                .ForMember(tsdto => tsdto.Level, opt => opt.MapFrom(vps => vps.Level))
                .ForMember(tsdto => tsdto.Name, opt => opt.MapFrom(vps => vps.TechSkill1.Name))
                .ForMember(tsdto => tsdto.Picture, opt => opt.MapFrom(vps => vps.TechSkill1.Picture));

            CreateMap<VacancySecondarySkill, TechSkillDTO>()
                .ForMember(tsdto => tsdto.Id, opt => opt.MapFrom(vps => vps.TechSkill1.Id))
                .ForMember(tsdto => tsdto.Level, opt => opt.MapFrom(vps => vps.Level))
                .ForMember(tsdto => tsdto.Name, opt => opt.MapFrom(vps => vps.TechSkill1.Name))
                .ForMember(tsdto => tsdto.Picture, opt => opt.MapFrom(vps => vps.TechSkill1.Picture));

            CreateMap<Vacancy, VacancyPreviewDTO>()
                .ForMember(v => v.City, opt => opt.MapFrom(dbv => dbv.City1.Name))
                .ForMember(v => v.PrimarySkill, opt => opt.MapFrom(dbv => Mapper.Map<VacancyPrimarySkill, TechSkillDTO>(dbv.VacancyPrimarySkill)))
                .ForMember(v => v.ProjectName, opt => opt.MapFrom(dbv => dbv.ProjectName))
                .ForMember(v => v.Status, opt => opt.MapFrom(dbv => dbv.VacancyStatus.Name));

            CreateMap<Vacancy, VacancyDTO>()
                .ForMember(v => v.Candidates, opt => opt.MapFrom(dbv => Mapper.Map<ICollection<Candidate>,
                                                                ICollection<CandidatePreviewDTO>>(dbv.Candidates)))
                .ForMember(v => v.City, opt => opt.MapFrom(dbv => dbv.City1.Name))
                .ForMember(v => v.EngLevel, opt => opt.MapFrom(dbv => dbv.EngLevel1.Name))
                .ForMember(v => v.HRM, opt => opt.MapFrom(dbv => Mapper.Map<User, UserPreviewDTO>(dbv.User)))
                .ForMember(v => v.ProjectName, opt => opt.MapFrom(dbv => dbv.ProjectName))
                .ForMember(v => v.PrimarySkill, opt => opt.MapFrom(dbv => Mapper.Map<VacancyPrimarySkill, TechSkillDTO>(dbv.VacancyPrimarySkill)))
                .ForMember(v => v.SecondarySkills, opt => opt.MapFrom(dbv => Mapper.Map<ICollection<VacancySecondarySkill>, ICollection<TechSkillDTO>>(dbv.VacancySecondarySkills)))
                .ForMember(v => v.Status, opt => opt.MapFrom(dbv => dbv.VacancyStatus.Name))
                .ForMember(v => v.VacancyName, opt => opt.MapFrom(dbv => dbv.VacancyName));

            CreateMap<TechSkillInDTO, VacancyPrimarySkill>()
                .ForMember(vps => vps.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));

            CreateMap<TechSkillInDTO, VacancySecondarySkill>()
                .ForMember(vps => vps.TechSkill, opt => opt.MapFrom(tsin => tsin.Id));

            CreateMap<VacancyInputDTO, Vacancy>()
                .ForMember(vac => vac.ProjectName, opt => opt.MapFrom(vin => vin.ProjectName))
                .ForMember(vac => vac.Candidates, opt => opt.Ignore())
                .ForMember(vac => vac.VacancyPrimarySkill, opt => opt.MapFrom(vin => vin.PrimarySkill))
                .ForMember(vac => vac.VacancySecondarySkills, opt => opt.MapFrom( vin => 
                Mapper.Map<ICollection<TechSkillInDTO>, ICollection<VacancySecondarySkill>>(vin.SecondarySkills)));

            CreateMap<VacancyInputDTO, VacancySearchModel>()
                .ForMember(vsm => vsm.PrimarySkill, opt => opt.MapFrom(vin => vin.PrimarySkill));
        }
    }
}