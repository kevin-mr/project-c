using ProjectC.Shared.Models;

namespace ProjectC.Server.Services
{
    public interface IRequestInspectorService
    {
        Task<RequestDto> BuildRequestAsync(HttpRequest httpRequest);
    }
}
