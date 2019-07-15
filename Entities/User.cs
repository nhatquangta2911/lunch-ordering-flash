using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities {
    public class User 
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage="{0} length must be between {2} and {1}", MinimumLength = 5)]
        public string Username { get; set;}
        
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage="{0} length must be between {2} and {1}", MinimumLength = 5)]
        public string Name { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        public bool IsAdmin { get; set; } 
    }
}