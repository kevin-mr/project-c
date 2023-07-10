using AutoMapper;
using ProjectC.Shared.Models;

namespace ProjectC.Shared.Mappers
{
    public class WorkflowStorageDtoProfile : Profile
    {
        public WorkflowStorageDtoProfile()
        {
            CreateMap<WorkflowStorageDto, EditWorkflowStorageDto>().ReverseMap();
            CreateMap<WorkflowStorageDto, CreateWorkflowStorageDto>().ReverseMap();
        }
    }
}
