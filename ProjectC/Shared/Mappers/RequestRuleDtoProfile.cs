using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class RequestRuleDtoProfile : Profile
    {
        public RequestRuleDtoProfile()
        {
            CreateMap<RequestRuleDto, EditRequestRuleDto>().ReverseMap();
            CreateMap<RequestRuleDto, CreateRequestRuleDto>().ReverseMap();
        }
    }
}
