using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities
{
    public class Dish
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage="{0} length must be between {2} and {1}", MinimumLength = 4)]
        public string Name { get; set; }

        public string Image { get; set; } = "https://coffeerani.com/wp-content/uploads/2018/08/meal-icon-200.png";

        public string Type { get; set; }
        
        [StringLength(255, ErrorMessage="{0} length must be between {2} and {1}", MinimumLength = 10)]
        public string Description { get; set; } = "Delicious Dish";

    }
}