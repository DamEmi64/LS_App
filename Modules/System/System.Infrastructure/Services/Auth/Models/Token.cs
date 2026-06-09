namespace System.Infrastructure.Services.Auth.Models
{
    public class Token
    {
        public required string Value { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }
    }
}
