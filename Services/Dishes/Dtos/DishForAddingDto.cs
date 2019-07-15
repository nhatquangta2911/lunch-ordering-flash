namespace CourseApi.Services.Dishes.Dtos
{
    public class DishForAddingDto
    {
        public string Name { get; set; }

        public string Image { get; set; } = "https://coffeerani.com/wp-content/uploads/2018/08/meal-icon-200.png";

        public string Type { get; set; }
        
        public string Description { get; set; } 
    }
}