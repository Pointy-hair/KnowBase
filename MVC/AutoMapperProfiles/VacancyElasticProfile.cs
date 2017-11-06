using System;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch.ElasticModels;
using MVC.Models;
using MVC.Models.OutDTO;

namespace MVC.AutoMapperProfiles
{
    public class VacancyElasticProfile: Profile
    {
        public VacancyElasticProfile()
        {
            CreateMap<VacancyPrimarySkill, SkillElasticModel>()
                .ForMember(vc => vc.Name, op => op.MapFrom(vc => vc.TechSkill1 != null
                    ? vc.TechSkill1.Name
                    : null));
            CreateMap<SkillElasticModel, TechSkillDTO>()
                .ForMember(cn => cn.Id, op => op.MapFrom(el => el.TechSkill));

            CreateMap<City, CityElasticModel>();
            CreateMap<VacancyStatus, StatusElasticModel>();

            CreateMap<Vacancy, VacancyElasticModel>()
                .ForMember(el => el.Status, op => op.MapFrom(v =>
                    Mapper.Map<VacancyStatus, StatusElasticModel>(v.VacancyStatus)))
                .ForMember(el => el.RequestDate, op => op.MapFrom(v => v.RequestDate ?? DateTime.MinValue))
                .ForMember(el => el.StartDate, op => op.MapFrom(v => v.StartDate ?? DateTime.MinValue))
                .ForMember(el => el.CloseDate, op => op.MapFrom(v => v.CloseDate ?? DateTime.MinValue))
                .ForMember(el => el.City, op => op.MapFrom(v => Mapper.Map<City, CityElasticModel>(v.City1)))
                .ForMember(el => el.PrimarySkill, op => op.MapFrom(v =>
                    Mapper.Map<VacancyPrimarySkill, SkillElasticModel>(v.VacancyPrimarySkill)));

            CreateMap<VacancyElasticModel, VacancyPreviewDTO>()
                .ForMember(pr => pr.StartDate, op => op.MapFrom(el =>
                    (el.StartDate != DateTime.MinValue)
                        ? new DateTime?(el.StartDate)
                        : null))
                .ForMember(pr => pr.CloseDate, op => op.MapFrom(el =>
                    el.CloseDate != DateTime.MinValue
                        ? new DateTime?(el.CloseDate)
                        : null))
                .ForMember(pr => pr.City, op => op.MapFrom(el =>
                    el.City != null
                        ? el.City.Name
                        : null))
                .ForMember(pr => pr.Status, op => op.MapFrom(el =>
                    el.Status != null
                        ? el.Status.Name
                        : null))
                .ForMember(pr => pr.PrimarySkill, op => op.MapFrom(el =>
                    Mapper.Map<SkillElasticModel,TechSkillDTO>(el.PrimarySkill)));
        }
    }
}