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

        public string image { get; set; }
    }
}