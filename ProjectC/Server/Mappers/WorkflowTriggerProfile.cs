using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WorkflowTriggerProfile : Profile
    {
        public WorkflowTriggerProfile()
        {
            CreateMap<WorkflowTrigger, WorkflowTriggerDto>()
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
            CreateMap<CreateWorkflowTriggerDto, WorkflowTrigger>().ReverseMap();
            CreateMap<EditWorkflowTriggerDto, WorkflowTrigger>().ReverseMap();
        }
    }
}
