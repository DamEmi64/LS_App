namespace System.Infrastructure.Services.Auth.Models
{
    public class RefreshTokenModel
    {
        public required string UserId { get; set; }

        public required string RefreshToken { get; set; }
    }
}
