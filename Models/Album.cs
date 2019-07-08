using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Models
{
   public class Album
   { 
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public string Id { get; set; }
      public string Name { get; set; }
      public decimal Price { get; set; }
      public string Category { get; set; }
      public string Artist { get; set; }
   }
}