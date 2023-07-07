using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWorkflowService
    {
        Task<IEnumerable<Workflow>> GetAsync();
        Task CreateAsync(Workflow workflow);
        Task UpdateAsync(Workflow workflow);
        Task DeleteAsync(int id);
    }
}
