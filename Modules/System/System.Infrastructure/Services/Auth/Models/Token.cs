namespace System.Infrastructure.Services.Auth.Models
{
    public class Token
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string UserId { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    }
}
