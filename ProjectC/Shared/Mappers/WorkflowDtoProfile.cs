using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class WorkflowDtoProfile : Profile
    {
        public WorkflowDtoProfile()
        {
            CreateMap<WorkflowDto, EditWorkflowDto>().ReverseMap();
            CreateMap<WorkflowDto, CreateWorkflowDto>().ReverseMap();
        }
    }
}
