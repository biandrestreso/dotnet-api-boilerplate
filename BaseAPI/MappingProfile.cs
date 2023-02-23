using AutoMapper;
using BaseAPI.Entities.DTO;
using BaseAPI.Entities.Models;

namespace BaseAPI.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // eg. CreateMap<Source, Destination>();
            //
            CreateMap<User, UserDto>();
            CreateMap<UserCreationDto, User>();
        }
    }
}
