using System;
using CourseApi.Entities;

namespace CourseApi.Services.Orders.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }

        public DateTime DateOrdered { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public Menu Menu { get; set; }

        public DailyChoice DailyChoice { get; set; }
    }
}