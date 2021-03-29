using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices.AutoMapper
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonModel>()
                .ForMember(dest => dest.Parent1Id, opt => opt.MapFrom(src => src.ChildOfFamily.Parent1Id))
                .ForMember(dest => dest.Parent2Id, opt => opt.MapFrom(src => src.ChildOfFamily.Parent2Id));
        }
    }
}
