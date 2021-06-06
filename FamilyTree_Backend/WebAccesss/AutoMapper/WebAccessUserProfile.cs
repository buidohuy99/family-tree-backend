using AutoMapper;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccesss.Models;

namespace WebAccesss.AutoMapper
{
    public class WebAccessUserProfile : Profile
    {
        public WebAccessUserProfile()
        {
            CreateMap<ApplicationUser, WebAccessUserModel>();
            CreateMap<ApplicationUser, WebAccessUserUpdateModel>().ReverseMap();
        }
    }
}
