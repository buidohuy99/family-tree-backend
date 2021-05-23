using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
{
    class FamilyTreeProfile : Profile
    {
        public FamilyTreeProfile()
        {
            CreateMap<FamilyTree, FamilyTreeModel>()
                .ForMember(dest => dest.People, opt => opt.MapFrom(src => src.People))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Editors, opt => opt.MapFrom(src => src.Editors));

            CreateMap<FamilyTreeInputModel, FamilyTree>();
            CreateMap<FamilyTree, FamilyTreeUpdateResponseModel>();

            CreateMap<FamilyTree, FamilyTreeListItemModel>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Editors, opt => opt.MapFrom(src => src.Editors)); ;
        }
    }
}
