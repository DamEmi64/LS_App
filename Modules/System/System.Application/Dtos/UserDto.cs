namespace System.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public required string Role { get; set; }
        public required string[] Permissions { get; set; }
    }
}