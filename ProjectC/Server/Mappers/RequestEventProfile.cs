using AutoMapper;
using ProjectC.Server.Data.Entities;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Mappers
{
    public class RequestEventProfile : Profile
    {
        public RequestEventProfile()
        {
            CreateMap<RequestEvent, RequestEventDto>();
        }
    }
}
