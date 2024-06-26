﻿using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserIconDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<ApplicationUser, WebAccessUserModel>();
            CreateMap<ApplicationUser, WebAccessUserUpdateModel>().ReverseMap();
        }
    }
}
