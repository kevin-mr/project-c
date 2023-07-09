using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class WorkflowActionDtoProfile : Profile
    {
        public WorkflowActionDtoProfile()
        {
            CreateMap<WorkflowActionDto, EditWorkflowActionDto>().ReverseMap();
            CreateMap<WorkflowActionDto, CreateWorkflowActionDto>().ReverseMap();
        }
    }
}
