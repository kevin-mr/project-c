using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/request")]
    public class RequestController : ControllerBase
    {
        private readonly ProjectCDbContext _context;

        public RequestController(ProjectCDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<CustomRequestDto[]> Get()
        {
            return await _context.Requests
                .Select(
                    x =>
                        new CustomRequestDto
                        {
                            Id = x.Id,
                            Method = x.Method,
                            Body = x.Body,
                            Path = x.Path,
                        }
                )
                .ToArrayAsync();
        }

        [HttpPost()]
        public async Task Create(CreateRequestDto request)
        {
            _context.Requests.Add(
                new Request
                {
                    Method = (int)request.Method,
                    Path = request.Path,
                    Body = request.Body,
                }
            );
            await _context.SaveChangesAsync();
        }
    }
}
