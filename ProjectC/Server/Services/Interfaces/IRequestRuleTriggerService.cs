using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestRuleTriggerService
    {
        Task<IEnumerable<RequestRuleTrigger>> GetAsync();
        Task<IEnumerable<RequestRuleTrigger>> GetByRequestRuleVariantIdAsync(
            int requestRuleVariantId
        );
        Task CreateAsync(RequestRuleTrigger requestRuleTrigger);
        Task UpdateAsync(RequestRuleTrigger requestRuleTrigger);
        Task DeleteAsync(int id);
    }
}
