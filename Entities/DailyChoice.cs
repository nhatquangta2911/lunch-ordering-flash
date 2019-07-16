using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CourseApi.Entities
{
    public class DailyChoice
    {

        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [StringLength(40, ErrorMessage="{0} length must be between {2} and {1}", MinimumLength = 4)]
        public string Name { get; set; } = "Today's Choices";

        public ushort amountOfChoices { get; set; } = 0;

        public DateTime dateCreated { get; set; } = DateTime.UtcNow;

        [Required]
        [MinLength(2)]
        public HashSet<string> MenuIds { get; set; }

    }
}