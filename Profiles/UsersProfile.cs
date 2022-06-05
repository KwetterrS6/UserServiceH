using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // Source -> Target
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserCreatedDto, UserReadDto>();
            CreateMap<UserReadDto, UserCreatedDto>();
        }
    }

}