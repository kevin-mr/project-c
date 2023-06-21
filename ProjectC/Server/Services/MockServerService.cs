using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Services
{
    public class MockServerService : IMockServerService
    {
        public static readonly string CUSTOM_PREFIX = "/custom";
        private readonly ProjectCDbContext _context;

        public MockServerService(ProjectCDbContext context)
        {
            _context = context;
        }

        public async Task<Request?> FindRequest(HttpRequest httpRequest)
        {
            if (!httpRequest.Path.HasValue)
            {
                return null;
            }

            var method = GetRequestMethod(httpRequest.Method);
            var path = httpRequest.Path.Value.Remove(0, CUSTOM_PREFIX.Length);

            return await _context.Requests.FirstOrDefaultAsync(
                x => x.Method == (int)method && x.Path == path
            );
        }

        private static RequestMethod GetRequestMethod(string method)
        {
            return method switch
            {
                "GET" => RequestMethod.GET,
                "POST" => RequestMethod.POST,
                "PUT" => RequestMethod.PUT,
                "DELETE" => RequestMethod.DELETE,
                _ => throw new Exception("Invalid Method"),
            };
        }
    }
}
