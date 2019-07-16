using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Menus.Dtos;

namespace CourseApi.MapProfiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuResponseDto>();
            CreateMap<MenuResponseDto, Menu>();
        }
    }
}