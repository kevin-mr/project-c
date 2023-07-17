using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WebhookEventProfile : Profile
    {
        public WebhookEventProfile()
        {
            CreateMap<RequestEvent, WebhookEvent>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<WebhookEvent, WebhookEventDto>()
                .ForMember(
                    x => x.Path,
                    opt =>
                    {
                        opt.PreCondition(x => x.WebhookRule is not null);
                        opt.MapFrom(x => x.WebhookRule != null ? x.WebhookRule.Path : string.Empty);
                    }
                )
                .ForMember(
                    x => x.RedirectUrl,
                    opt =>
                    {
                        opt.PreCondition(x => x.WebhookRule is not null);
                        opt.MapFrom(x => x.WebhookRule!.RedirectUrl);
                    }
                );
        }
    }
}
