using AutoMapper;
using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Core.Domain.Entities;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
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
