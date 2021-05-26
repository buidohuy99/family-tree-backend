using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
{
    public class FamilyEventProfile : Profile
    {
        public FamilyEventProfile()
        {
            CreateMap<FamilyEventExceptionCase, FamilyEventExceptionCaseModel>().ReverseMap();
            CreateMap<FamilyEvent, FamilyEventModel>().ReverseMap();
            CreateMap<FamilyEventInputModel, FamilyEvent>().ReverseMap();
            CreateMap<FamilyEventUpdateModel, FamilyEvent>();
            CreateMap<FamilyEventUpdateModel, FamilyEventExceptionCase>();
            CreateMap<FamilyEvent, FamilyEventOutputModel>().ReverseMap();
        }
    }
}
