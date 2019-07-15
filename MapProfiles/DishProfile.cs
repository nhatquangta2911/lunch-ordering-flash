using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Dishes.Dtos;

namespace CourseApi.MapProfiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Dish, DishForAddingDto>();
            CreateMap<DishForAddingDto, Dish>();
            CreateMap<Dish, DishForResponseDto>();
        }
    }
}