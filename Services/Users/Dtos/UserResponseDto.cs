namespace CourseApi.Services.Users.Dtos
{
    public class UserResponseDto
    {
        public string Id { get; set; }

        public string Username { get; set;}

        public string Name { get; set; }

        public string Phone { get; set; }

        public bool IsAdmin { get; set; } 
    }
}