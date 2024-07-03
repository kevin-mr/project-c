using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class RequestEventProfile : Profile
    {
        public RequestEventProfile()
        {
            CreateMap<RequestEvent, RequestEventDto>()
                .ForMember(
                    x => x.Path,
                    opt =>
                    {
                        opt.MapFrom(
                            x =>
                                x.RequestRule != null
                                    ? x.RequestRule.Path
                                    : x.WebhookRule != null
                                        ? x.WebhookRule.Path
                                        : x.RequestRuleVariant != null
                                            ? x.RequestRuleVariant.Path
                                            : x.Path
                        );
                    }
                )
                .ForMember(
                    x => x.RedirectUrl,
                    opt =>
                    {
                        opt.PreCondition(x => x.WebhookRule is not null);
                        opt.MapFrom(x => x.WebhookRule!.RedirectUrl);
                    }
                )
                .ForMember(
                    x => x.RequestRuleVariantDescription,
                    opt =>
                    {
                        opt.PreCondition(x => x.RequestRuleVariant is not null);
                        opt.MapFrom(x => x.RequestRuleVariant!.Description);
                    }
                );
        }
    }
}
