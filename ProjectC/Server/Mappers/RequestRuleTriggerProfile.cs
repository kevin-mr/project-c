using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class RequestRuleTriggerProfile : Profile
    {
        public RequestRuleTriggerProfile()
        {
            CreateMap<RequestRuleTrigger, RequestRuleTriggerDto>()
                .ForMember(
                    x => x.WebhookRuleDescription,
                    opt =>
                    {
                        opt.PreCondition(
                            x =>
                                x.WebhookEvent is not null && x.WebhookEvent.WebhookRule is not null
                        );
                        opt.MapFrom(x => x.WebhookEvent!.WebhookRule!.Description);
                    }
                )
                .ReverseMap();
            CreateMap<CreateRequestRuleTriggerDto, RequestRuleTrigger>().ReverseMap();
            CreateMap<EditRequestRuleTriggerDto, RequestRuleTrigger>().ReverseMap();
        }
    }
}
