using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services
{
    public interface IMockServerService
    {
        Task<Request?> FindRequest(HttpRequest httpRequest);
    }
}
