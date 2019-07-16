using System;
using CourseApi.Entities;
using CourseApi.Services.Users.Dtos;

namespace CourseApi.Services.Orders.Dtos
{
    public class OrderResponseDto
    {
        public string Id { get; set; }

        public DateTime DateOrdered { get; set; } 
        
        public UserResponseDto User { get; set; }
        
        public Menu Menu { get; set; }
        
        public DailyChoice DailyChoice { get; set; }
    }
}