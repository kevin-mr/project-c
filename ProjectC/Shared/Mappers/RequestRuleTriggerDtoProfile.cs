using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class RequestRuleTriggerDtoProfile : Profile
    {
        public RequestRuleTriggerDtoProfile()
        {
            CreateMap<RequestRuleTriggerDto, EditRequestRuleTriggerDto>().ReverseMap();
            CreateMap<RequestRuleTriggerDto, CreateRequestRuleTriggerDto>().ReverseMap();
        }
    }
}
