using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mapper
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
