namespace System.Infrastructure.Services.Auth.Models
{
    public class ResetPasswordModel
    {
        public required string Login { get; set; }
        public required string Code { get; set; }
        public required string Password { get; set; }
    }
}
