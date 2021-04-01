using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
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
