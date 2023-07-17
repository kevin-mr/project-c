using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWorkflowActionService
    {
        Task<IEnumerable<WorkflowAction>> GetAsync();
        Task<IEnumerable<WorkflowAction>> GetByWorkflowIdAsync(int workflowId);
        Task CreateAsync(WorkflowAction workflowAction);
        Task UpdateAsync(WorkflowAction workflowAction);
        Task DeleteAsync(int id);
        Task ExecuteTriggersAsync(int workflowActionId);
    }
}
