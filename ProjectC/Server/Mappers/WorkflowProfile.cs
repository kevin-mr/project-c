using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkflowDto>().ReverseMap();
            CreateMap<CreateWorkflowDto, Workflow>().ReverseMap();
            CreateMap<EditWorkflowDto, Workflow>().ReverseMap();
        }
    }
}
