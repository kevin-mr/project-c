using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class RequestRuleVariantProfile : Profile
    {
        public RequestRuleVariantProfile()
        {
            CreateMap<RequestRuleVariant, RequestRuleVariantDto>().ReverseMap();
            CreateMap<CreateRequestRuleVariantDto, RequestRuleVariant>().ReverseMap();
            CreateMap<EditRequestRuleVariantDto, RequestRuleVariant>().ReverseMap();
        }
    }
}
