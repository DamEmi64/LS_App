using Microsoft.AspNetCore.Identity;

namespace System.Application.Dtos
{
    public class UserDetailsDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IdentityRole? Role { get; set; }
        public bool LockedOut { get; set; }
    }
}
