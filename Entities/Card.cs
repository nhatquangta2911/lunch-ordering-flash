using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities {
    public class Card {

        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }
        public string EnglishTitle { get; set; }
        public string VietnameseTitle { get; set; }
    }
}