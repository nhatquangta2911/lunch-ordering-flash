using System.Collections.Generic;
using CourseApi.Entities;

namespace CourseApi.Services.Menus.Dtos
{
    public class MenuResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public HashSet<Dish> Dishes { get; set; } 
    }
}