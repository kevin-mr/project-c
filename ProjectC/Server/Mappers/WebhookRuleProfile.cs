using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WebhookRuleProfile : Profile
    {
        public WebhookRuleProfile()
        {
            CreateMap<WebhookRule, WebhookRuleDto>().ReverseMap();

            CreateMap<WebhookRuleMethod, WebhookRuleMethodDto>()
                .ConvertUsingEnumMapping(x =>
                {
                    x.MapValue(WebhookRuleMethod.POST, WebhookRuleMethodDto.POST);
                    x.MapValue(WebhookRuleMethod.PUT, WebhookRuleMethodDto.PUT);
                })
                .ReverseMap();

            CreateMap<CreateWebhookRuleDto, WebhookRule>().ReverseMap();
            CreateMap<EditWebhookRuleDto, WebhookRule>().ReverseMap();
        }
    }
}
