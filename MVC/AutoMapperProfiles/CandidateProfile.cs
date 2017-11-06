using System.Collections.Generic;
using AutoMapper;
using BusinessLogic.DBContext;
using MVC.Models;
using MVC.Models.InDTO;
using MVC.Models.OutDTO;

namespace MVC.AutoMapperProfiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, CandidatePreviewDTO>()
                .ForMember(cnDTO => cnDTO.Phone, opt => opt.MapFrom(cn => cn.Contact.Phone))
                .ForMember(cnDTO => cnDTO.Email, opt => opt.MapFrom(cn => cn.Contact.Email))
                .ForMember(cnDTO => cnDTO.PrimarySkill, opt => opt.MapFrom(cn => cn.CandidatePrimarySkill.TechSkill1.Name))
                .ForMember(cnDTO => cnDTO.Status, opt => opt.MapFrom(cn => cn.CandidateStatus.Name))
                .ForMember(cnDTO => cnDTO.Skype, opt => opt.MapFrom(cn => cn.Contact.Skype));

            CreateMap<Candidate, CandidateDTO>()
                .ForMember(cnDTO => cnDTO.EngLevel,
                    opt => opt.MapFrom(cn => cn.EngLevel1.Name))
                .ForMember(cnDTO => cnDTO.HRM, opt => opt.MapFrom(cn => Mapper.Map<User, UserPreviewDTO>(cn.User)))
                .ForMember(cnDTO => cnDTO.CandidatePrimarySkill,
                    opt => opt.MapFrom(cn => Mapper.Map<CandidatePrimarySkill, TechSkillDTO>(cn.CandidatePrimarySkill)))
                .ForMember(cnDTO => cnDTO.CandidateSecondarySkills,
                    opt => opt.MapFrom(
                        cn => Mapper.Map<ICollection<CandidateSecondarySkill>, List<TechSkillDTO>>(cn
                            .CandidateSecondarySkills)))
                .ForMember(cnDTO => cnDTO.Contact,
                    opt => opt.MapFrom(cn => Mapper.Map<Contact, ContactsDTO>(cn.Contact)))
                .ForMember(cnDTO => cnDTO.City, opt => opt.MapFrom(cn => cn.City1.Name))
                .ForMember(cnDTO => cnDTO.Status,
                    opt => opt.MapFrom(cn => cn.CandidateStatus.Name))
                .ForMember(cnDTO => cnDTO.LastModifier,
                    opt => opt.MapFrom(cn => Mapper.Map<User, UserPreviewDTO>(cn.User1)))
                .ForMember(cnDTO => cnDTO.CandidatePrevJobsContacts,
                    opt => opt.MapFrom(cn =>
                        Mapper.Map<ICollection<CandidatePrevJobsContact>, List<CandidatePrevJobDTO>>(cn
                            .CandidatePrevJobsContacts)))
                .ForMember(cnDTO => cnDTO.Vacancies, 
                    opt => opt.MapFrom(cn => Mapper.Map<ICollection<Vacancy>, List<VacancyPreviewDTO>>(cn.Vacancies)));

            CreateMap<CandidatePrevJobDTO, CandidatePrevJobsContact>()
                .ForMember(cnPrDTO => cnPrDTO.Contact,
                    opt => opt.MapFrom(cont => Mapper.Map<ContactsDTO, Contact>(cont.Contact)));

            CreateMap<CandidatePrevJobsContact, CandidatePrevJobDTO>()
                .ForMember(cn => cn.Contact, opt => opt.MapFrom(cnPR => Mapper.Map<Contact, ContactsDTO>(cnPR.Contact)));

            CreateMap<CandidateInputDTO, Candidate>()
                .ForMember(cn => cn.Contact,
                    opt => opt.MapFrom(cnPrDTO => Mapper.Map<ContactsDTO, Contact>(cnPrDTO.Contact)))
                .ForMember(cn => cn.CandidatePrevJobsContacts,
                    opt => opt.MapFrom(cnDTO => Mapper.Map<List<CandidatePrevJobDTO>,
                        IEnumerable<CandidatePrevJobsContact>>(cnDTO.CandidatePrevJobs)))
                .ForMember(cn => cn.CandidateSecondarySkills, opt => opt.MapFrom(cnDTO =>
                    Mapper.Map<List<TechSkillInDTO>, IEnumerable<CandidateSecondarySkill>>(cnDTO.CandidateSecondarySkills)))
                .ForMember(cn => cn.CandidatePrimarySkill, opt => opt.MapFrom(cnDTO => Mapper.Map<TechSkillInDTO, CandidatePrimarySkill>(cnDTO.CandidatePrimarySkill)));
        }
    }
}