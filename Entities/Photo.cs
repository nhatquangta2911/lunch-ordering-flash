using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public string DishId { get; set; }
    }
}