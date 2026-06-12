namespace Events.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string? UserId { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }

        public bool Present { get; set; }
    }
}
