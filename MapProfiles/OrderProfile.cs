using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Orders.Dtos;

namespace CourseApi.MapProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderResponseDto, Order>();
        }
    }
}