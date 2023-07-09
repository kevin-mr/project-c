using AutoMapper;
using ProjectC.Server.Models;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WebhookEventProfile : Profile
    {
        public WebhookEventProfile()
        {
            CreateMap<WebhookEvent, WebhookEventDto>().ReverseMap();
        }
    }
}
