using Base;
using Communication.Domain;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Connect.SendEmail.Strategies;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Communication.Infrastructure.Connect.SendEmail
{
    public class EmailSender : ConnectInstance<Base.SendEmail>
    {
        private readonly EmailOptions _options;
        private readonly ICommunicationHistoryRepository _mailHistoryRepository;
        private readonly List<ISendStrategy> _sendStrategies;

        public EmailSender(IOptions<EmailOptions> options,
            ICommunicationHistoryRepository mailHistoryRepository,
            List<ISendStrategy> sendStrategies)
        {
            _options = options.Value;
            _mailHistoryRepository = mailHistoryRepository;
            _sendStrategies = sendStrategies;
        }


        public override Task<Result> HandleAsync(Base.SendEmail request)
            => SendEmailAsync(request.To, request.Subject, request.Body, request.From, request.MessageId);

        private async Task<Result> SendEmailAsync(string to, string subject, string body, string? from = null, string? messageId = null)
        {
            try
            {
                var strategy = _sendStrategies.FirstOrDefault(x => x.Mode == _options.Mode);

                ArgumentNullException.ThrowIfNull(strategy);

                var result = await strategy.Send(to, subject, body, from, messageId);

                if (result.IsSuccess)
                {
                    await _mailHistoryRepository.Add(new Domain.Entities.CommunicationHistory
                    {
                        Body = body,
                        Subject = subject,
                        Recipient = to,
                        Date = DateTime.Now
                    });
                }

                return result;

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
