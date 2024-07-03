using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class WebhookRuleDtoProfile : Profile
    {
        public WebhookRuleDtoProfile()
        {
            CreateMap<WebhookRuleDto, EditWebhookRuleDto>().ReverseMap();
            CreateMap<WebhookRuleDto, CreateWebhookRuleDto>().ReverseMap();
        }
    }
}
