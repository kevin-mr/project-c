using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWorkflowTriggerService
    {
        Task<IEnumerable<WorkflowTrigger>> GetAsync();
        Task<IEnumerable<WorkflowTrigger>> GetByWorkflowActionIdAsync(int workflowActionId);
        Task CreateAsync(WorkflowTrigger workflowTrigger);
        Task UpdateAsync(WorkflowTrigger workflowTrigger);
        Task DeleteAsync(int id);
    }
}
