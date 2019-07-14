using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CourseApi.Entities
{
   public class Album
   { 
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public string Id { get; set; }
      [BsonElement("Name")]
      [JsonProperty]
      public string Name { get; set; }
      public decimal Price { get; set; }
      public string Category { get; set; }
      public string Artist { get; set; }
   }
}