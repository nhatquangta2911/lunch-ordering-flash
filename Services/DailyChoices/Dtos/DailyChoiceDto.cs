using System;
using System.Collections.Generic;
using CourseApi.Entities;

namespace CourseApi.Services.DailyChoices.Dtos
{
    public class DailyChoiceDto
    {
        public string Id { get; set; }

        public string Name { get; set; } = "Today's Choices";

        public ushort amountOfChoices { get; set; } = 0;
        
        public DateTime dateCreated { get; set; }
    
        public HashSet<Menu> Menus { get; set; }
    }
}