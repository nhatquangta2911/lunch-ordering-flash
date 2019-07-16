using CourseApi.Entities;

namespace CourseApi.Services.Orders.Dtos
{
    public class OrderResponseDto
    {
        public User User { get; set; }

        public Menu Menu { get; set; }

        public DailyChoice DailyChoice { get; set; }
    }
}