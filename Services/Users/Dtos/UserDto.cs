namespace CourseApi.Services.Users.Dtos
{
    public class UserDto
    {
        public string Username { get; set;}

        public string Password { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; } 

        public string Token { get; set; }
    }
}