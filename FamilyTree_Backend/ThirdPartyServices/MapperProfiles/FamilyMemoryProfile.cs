using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
{
    public class FamilyMemoryProfile : Profile
    {
        public FamilyMemoryProfile()
        {
            CreateMap<FamilyMemory, FamilyMemoryModel>();
            CreateMap<FamilyMemoryInputModel, FamilyMemory>();
        }
    }
}
