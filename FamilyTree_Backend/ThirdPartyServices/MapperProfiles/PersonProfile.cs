using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.Person;
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
            CreateMap<Person, FileIOPersonDTO>()
               .ForMember(dest => dest.Parent1Id, opt => opt.MapFrom(src => src.ChildOfFamily.Parent1Id))
               .ForMember(dest => dest.Parent2Id, opt => opt.MapFrom(src => src.ChildOfFamily.Parent2Id))
               .ForMember(dest => dest.ChildOfCoupleId, opt => opt.MapFrom(src => src.ChildOf));

            CreateMap<PersonInputModel, Person>();

            CreateMap<Person, PersonDetailsModel>()
                .ForMember(dest => dest.Father, opt => opt.MapFrom(src => src.ChildOfFamily.Parent1))
                .ForMember(dest => dest.Mother, opt => opt.MapFrom(src => src.ChildOfFamily.Parent2));
            CreateMap<Relationship, RelationshipDTO>();
            CreateMap<Relationship, FileIOSpouseDTO.FileIOSpouseRelationshipDTO>().ReverseMap();
            CreateMap<Person, PersonSummaryDTO>();
            CreateMap<SpouseRelationshipUpdateModel, Relationship>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap()
                .ForMember(dest => dest.SpouseId, opt => opt.Ignore());
            CreateMap<PersonDetailsUpdateModel, Person>();
            CreateMap<Person, PersonDetailsResponseModel>();
        }
    }
}
