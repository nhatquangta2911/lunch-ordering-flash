using System;
using System.Collections.Generic;
using CourseApi.Entities;
using CourseApi.Services.Menus.Dtos;

namespace CourseApi.Services.DailyChoices.Dtos
{
    public class DailyChoiceDto
    {
      public DailyChoiceDto(string id, string name, ushort amountOfChoices, DateTime dateCreated) 
        {
           this.Id = id;
               this.Name = name;
               this.amountOfChoices = amountOfChoices;
               this.dateCreated = dateCreated;
               
        }
                public string Id { get; set; }

        public string Name { get; set; } = "Today's Choices";

        public ushort amountOfChoices { get; set; } = 0;
        
        public DateTime dateCreated { get; set; }
    
        public HashSet<MenuResponseDto> Menus { get; set; }
    }
}