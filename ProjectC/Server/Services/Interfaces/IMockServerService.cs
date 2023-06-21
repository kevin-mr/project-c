using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IMockServerService
    {
        Task<RequestRule?> FindRequestRule(HttpRequest httpRequest);
        Task BuildRequestRuleResponse(HttpContext context, RequestRule requestRule);
    }
}
