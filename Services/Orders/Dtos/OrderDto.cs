using System;
using CourseApi.Entities;
using CourseApi.Services.DailyChoices.Dtos;

namespace CourseApi.Services.Orders.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }

        public DateTime dateOrdered { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public Menu Menu { get; set; }

        public DailyChoice DailyChoice { get; set; }
    }
}