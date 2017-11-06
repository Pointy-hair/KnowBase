using System;
using AutoMapper;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch.ElasticModels;
using MVC.Models.OutDTO;

namespace MVC.AutoMapperProfiles
{
    public class CandidateElasticProfile : Profile
    {
        public CandidateElasticProfile()
        {
            CreateMap<CandidatePrimarySkill, SkillElasticModel>()
                .ForMember(el => el.Name, opt => opt.MapFrom(cn => cn.TechSkill1 != null? cn.TechSkill1.Name: null));
            CreateMap<CandidateStatus, StatusElasticModel>();

            CreateMap<Candidate, CandidateElasticModel>()
                .ForMember(el => el.LastContactDate, op => op.MapFrom(cn => cn.LastContactDate ?? DateTime.MinValue))
                .ForMember(el => el.RemindDate, op => op.MapFrom(cn => cn.Reminder ?? DateTime.MinValue))
                .ForMember(el => el.PrimarySkill, op => op.MapFrom(cn =>
                    Mapper.Map<CandidatePrimarySkill, SkillElasticModel>(cn.CandidatePrimarySkill)))
                .ForMember(el => el.Status, op => op.MapFrom(cn =>
                    Mapper.Map<CandidateStatus, StatusElasticModel>(cn.CandidateStatus)))
                .ForMember(el => el.City, op => op.MapFrom(cn => cn.City ?? 0))
                .ForMember(el => el.Email, op => op.MapFrom(cn => (cn.Contact != null) ? cn.Contact.Email : null))
                .ForMember(el => el.Phone, op => op.MapFrom(cn => (cn.Contact != null) ? cn.Contact.Phone : null))
                .ForMember(el => el.Skype, op => op.MapFrom(cn => (cn.Contact != null) ? cn.Contact.Skype : null));

            CreateMap<CandidateElasticModel, CandidatePreviewDTO>()
                .ForMember(pr => pr.Status, op => op.MapFrom(el => (el.Status != null) ? el.Status.Name : null))
                .ForMember(pr => pr.PrimarySkill, op => op.MapFrom(el =>
                    (el.PrimarySkill != null) ? el.PrimarySkill.Name : null));
        }
    }
}