using Communication.Domain;
using FluentResults;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Communication.Infrastructure.Connect.SendEmail.Strategies
{
    public class SendViaSMTPStrategy : ISendStrategy
    {
        private readonly EmailOptions _options;

        public SendViaSMTPStrategy(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public string Mode => "smtp";

        public async Task<Result> Send(string to, string subject, string body, string? from = null, string? messageId = null)
        {
            try
            {
                var mail = new MailMessage();

                SmtpClient client = new(_options.SmtpServer, _options.SmtpPort) { EnableSsl = false };

                if (!string.IsNullOrEmpty(_options.PublicKey) && !string.IsNullOrEmpty(_options.PrivateKey))
                {
                    client = new SmtpClient(_options.SmtpServer, _options.SmtpPort) //Port 8025, 587 and 25 can also be used.
                    {
                        Credentials = new NetworkCredential(_options.PublicKey, _options.PrivateKey),
                        EnableSsl = true
                    };
                }

                mail.Sender = new MailAddress(_options.ApiEmail, from);
                mail.From = new MailAddress(_options.ApiEmail, from);

                mail.To.Add(to);
                mail.Subject = subject;
                var plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);
                await client.SendMailAsync(mail);

                return Result.Ok();

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
