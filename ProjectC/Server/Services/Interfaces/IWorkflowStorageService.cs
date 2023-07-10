using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWorkflowStorageService
    {
        Task<IEnumerable<WorkflowStorage>> GetAsync();
        Task<WorkflowStorage?> GetByWorkflowIdAsync(int workflowId);
        Task CreateAsync(WorkflowStorage workflowStorage);
        Task UpdateAsync(WorkflowStorage workflowStorage);
        Task DeleteAsync(int id);
    }
}
