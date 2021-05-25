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
            CreateMap<FamilyEvent, FamilyEventModel>().ForMember(e => e.EventHistories, opt => opt.MapFrom(src => src.EventHistories)).ReverseMap();
            CreateMap<FamilyEventInputModel, FamilyEvent>().ReverseMap();
            CreateMap<FamilyEventHistory, FamilyEventHistoryModel>().ReverseMap();
            CreateMap<FamilyEventUpdateModel, FamilyEvent>();

            CreateMap<FamilyEventHistoryInputModel, FamilyEventHistory>().ReverseMap();
        }
    }
}
