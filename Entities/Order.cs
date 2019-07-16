using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime DateOrdered { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; }

        [Required]
        public string MenuId { get; set; }

        [Required]
        public string DailyChoiceId { get; set; }
    }
}