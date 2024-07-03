using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestRuleVariantService
    {
        Task<IEnumerable<RequestRuleVariant>> GetAsync();
        Task<IEnumerable<RequestRuleVariant>> GetByWorkflowIdAsync(int workflowId);
        Task CreateAsync(RequestRuleVariant requestRuleVariant);
        Task UpdateAsync(RequestRuleVariant requestRuleVariant);
        Task DeleteAsync(int id);
    }
}
