using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities {
    public class User 
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set;}

        public string Password { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; } 

        public string Token { get; set; }
    }
}