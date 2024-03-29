using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Users.Dtos;

namespace CourseApi.MapProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {   
            CreateMap<UserDto, User>();
            CreateMap<UserAuthDto, User>();
            CreateMap<UserResponseDto, User>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserResponseDto>();
        }   
    }
}