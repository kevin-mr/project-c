using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mapper
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
