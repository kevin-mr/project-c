using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class WorkflowStorageProfile : Profile
    {
        public WorkflowStorageProfile()
        {
            CreateMap<WorkflowStorage, WorkflowStorageDto>().ReverseMap();
            CreateMap<CreateWorkflowStorageDto, WorkflowStorage>().ReverseMap();
            CreateMap<EditWorkflowStorageDto, WorkflowStorage>().ReverseMap();
        }
    }
}
