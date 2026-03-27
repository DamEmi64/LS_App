using Microsoft.AspNetCore.Identity;

namespace System.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset InsDate { get; set; }
    }
}