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
                        opt.PreCondition(
                            x =>
                                x.RequestRule is not null
                                || x.WebhookRule is not null
                                || x.WorkflowAction is not null
                        );
                        opt.MapFrom(
                            x =>
                                x.RequestRule != null
                                    ? x.RequestRule.Path
                                    : x.WebhookRule != null
                                        ? x.WebhookRule.Path
                                        : x.WorkflowAction != null
                                            ? x.WorkflowAction.Path
                                            : string.Empty
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
                    x => x.WorkflowActionDescription,
                    opt =>
                    {
                        opt.PreCondition(x => x.WorkflowAction is not null);
                        opt.MapFrom(x => x.WorkflowAction!.Description);
                    }
                );
        }
    }
}
