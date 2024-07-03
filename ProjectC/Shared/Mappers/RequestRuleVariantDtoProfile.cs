using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class RequestRuleVariantDtoProfile : Profile
    {
        public RequestRuleVariantDtoProfile()
        {
            CreateMap<RequestRuleVariantDto, EditRequestRuleVariantDto>().ReverseMap();
            CreateMap<RequestRuleVariantDto, CreateRequestRuleVariantDto>().ReverseMap();
        }
    }
}
