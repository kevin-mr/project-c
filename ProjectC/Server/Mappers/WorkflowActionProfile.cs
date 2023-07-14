using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WorkflowActionProfile : Profile
    {
        public WorkflowActionProfile()
        {
            CreateMap<WorkflowAction, WorkflowActionDto>().ReverseMap();
            CreateMap<CreateWorkflowActionDto, WorkflowAction>().ReverseMap();
            CreateMap<EditWorkflowActionDto, WorkflowAction>().ReverseMap();
        }
    }
}
