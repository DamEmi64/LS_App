namespace System.Infrastructure.Services.Auth.Models
{
    public class ChangePasswordModel
    {
        public string? UserId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}