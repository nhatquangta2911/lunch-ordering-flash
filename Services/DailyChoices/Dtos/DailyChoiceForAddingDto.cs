using System.Collections.Generic;

namespace CourseApi.Services.DailyChoices.Dtos
{
    public class DailyChoiceForAddingDto
    {
        public string Name { get; set; } = "Today's Choices";

        public HashSet<string> MenuIds { get; set; }
    }
}