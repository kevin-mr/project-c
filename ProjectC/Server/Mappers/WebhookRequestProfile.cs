using AutoMapper;
using ProjectC.Server.Models;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WebhookRequestProfile : Profile
    {
        public WebhookRequestProfile()
        {
            CreateMap<WebhookRequest, WebhookRequestDto>().ReverseMap();
        }
    }
}
