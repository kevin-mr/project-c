using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class RequestRuleProfile : Profile
    {
        public RequestRuleProfile()
        {
            CreateMap<RequestRule, RequestRuleDto>().ReverseMap();
            CreateMap<RequestRuleMethod, RequestRuleMethodDto>()
                .ConvertUsingEnumMapping(x =>
                {
                    x.MapValue(RequestRuleMethod.GET, RequestRuleMethodDto.GET);
                    x.MapValue(RequestRuleMethod.POST, RequestRuleMethodDto.POST);
                    x.MapValue(RequestRuleMethod.PUT, RequestRuleMethodDto.PUT);
                    x.MapValue(RequestRuleMethod.DELETE, RequestRuleMethodDto.DELETE);
                })
                .ReverseMap();
            CreateMap<CreateRequestRuleDto, RequestRule>().ReverseMap();
            CreateMap<EditRequestRuleDto, RequestRule>().ReverseMap();
            CreateMap<RequestRuleMethodCounter, RequestRuleMethodCounterDto>().ReverseMap();
        }
    }
}
