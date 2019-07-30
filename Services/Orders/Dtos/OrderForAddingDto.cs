using System.ComponentModel.DataAnnotations;
using CourseApi.Entities;

namespace CourseApi.Services.Orders.Dtos
{
    public class OrderForAddingDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string MenuId { get; set; }

        [Required]
        public string DailyChoiceId { get; set; }
    }
}