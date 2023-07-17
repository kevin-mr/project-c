using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class WorkflowTriggerDtoProfile : Profile
    {
        public WorkflowTriggerDtoProfile()
        {
            CreateMap<WorkflowTriggerDto, EditWorkflowTriggerDto>().ReverseMap();
            CreateMap<WorkflowTriggerDto, CreateWorkflowTriggerDto>().ReverseMap();
        }
    }
}
