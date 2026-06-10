using Communication.Domain;
using FluentResults;
using Mailjet.Client;
using Mailjet.Client.Resources;

namespace Communication.Infrastructure.Connect.SendEmail.Strategies
{
    public class SendViaMailjetApiStrategy : ISendStrategy
    {
        private readonly EmailOptions _options;

        public SendViaMailjetApiStrategy(EmailOptions options)
        {
            _options = options;
        }

        public string Mode => "Mailjet";

        public async Task<Result> Send(string to, string subject, string body, string? from = null, string? messageId = null)
        {
            try
            {
                MailjetClient client = new MailjetClient(_options.PublicKey, _options.PrivateKey);
                var request = new MailjetRequest
                {
                    Resource = SendV31.Resource,
                }
                .Property(Mailjet.Client.Resources.Send.Messages, new[]
                {
                    new
                    {
                        From = new
                        {
                            Email = _options.ApiEmail,
                            Name = from
                        },
                        To = new[]
                        {
                            new
                            {
                                Email = to
                            }
                        },
                        Subject = subject,
                        HtmlPart = body,
                        TextPart = body,
                        CustomID = messageId
                    }
                });

                var response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail(response.GetErrorMessage());
                }

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}