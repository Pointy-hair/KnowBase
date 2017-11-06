using AutoMapper;
using BusinessLogic.DBContext;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserPreviewDTO>()
                .ForMember(v => v.Role, opt => opt.MapFrom(us => us.UserRole.Name));
            CreateMap<UserPreviewDTO, User>()
                .ForMember(us => us.UserRole, opt => opt.MapFrom(prDTO =>new UserRole{Name = prDTO.Role}));
        }
    }
}