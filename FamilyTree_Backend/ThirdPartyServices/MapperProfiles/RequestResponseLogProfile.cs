using AutoMapper;
using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles
{
    public class RequestResponseLogProfile : Profile
    {
        public RequestResponseLogProfile()
        {
            CreateMap<RequestResponseDataModel, RequestResponseListModel>()
                .ForMember(dest => dest.StatusCode, opt =>
                    opt.MapFrom(src => $"{src.StatusCode}-{ReasonPhrases.GetReasonPhrase(src.StatusCode)}"));

            CreateMap<RequestResponseDataModel, RequestResponseLogDetailsModel>()
                .ForMember(dest => dest.StatusCode, opt =>
                    opt.MapFrom(src => $"{src.StatusCode}-{ReasonPhrases.GetReasonPhrase(src.StatusCode)}"))
                .ForMember(dest => dest.RequestBody, opt =>
                    opt.MapFrom(src => PretifyJsonString(src.RequestBody)))
                .ForMember(dest => dest.ResponseBody, opt =>
                    opt.MapFrom(src => PretifyJsonString(src.ResponseBody)));
        }

        private static string PretifyJsonString(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            string result = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            return result;
        }
    }
}
