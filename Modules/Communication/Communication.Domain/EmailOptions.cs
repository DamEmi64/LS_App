namespace Communication.Domain
{
    public class EmailOptions
    {
        public required string SmtpServer { get; set; } = "mail.smtp2go.com";
        public int SmtpPort { get; set; } = 2525; // Default port for SMTP2GO, can also use 8025, 587, or 25
        public required string PublicKey { get; set; }
        public required string PrivateKey { get; set; }
        public required string ApiEmail { get; set; }
    }
}