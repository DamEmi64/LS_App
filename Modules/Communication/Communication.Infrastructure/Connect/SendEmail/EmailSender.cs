using Communication.Domain;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Connect.SendEmail.Strategies;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Communication.Infrastructure.Connect.SendEmail
{
    public class EmailSender : Base.EventHandler<Base.SendEmail>
    {
        private readonly EmailOptions _options;
        private readonly ICommunicationHistoryRepository _mailHistoryRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly List<ISendStrategy> _sendStrategies;

        public EmailSender(IOptions<EmailOptions> options,
            ICommunicationHistoryRepository mailHistoryRepository,
            IEnumerable<ISendStrategy> sendStrategies,
            IEmailRepository emailRepository)
        {
            _options = options.Value;
            _mailHistoryRepository = mailHistoryRepository;
            _sendStrategies = sendStrategies.ToList();
            _emailRepository = emailRepository;
        }


        public override Task<Result> HandleAsync(Base.SendEmail request, CancellationToken cancellationToken)
            => SendEmailAsync(request);

        private async Task<Result> SendEmailAsync(Base.SendEmail request)
        {
            try
            {
                var strategy = _sendStrategies.FirstOrDefault(x => x.Mode == _options.Mode);

                ArgumentNullException.ThrowIfNull(strategy);

                var result = await strategy.Send(request.To, request.Subject, request.Body, request.From, request.MessageId);

                if (result.IsSuccess)
                {
                    var messageId = request.MessageId;

                    if (string.IsNullOrEmpty(messageId) && request.Register)
                    {
                        var email = new Domain.Entities.Email
                        {
                            Body = request.Body,
                            Subject = request.Subject,
                            Sender = request.From ?? _options.ApiEmail,
                            Recipient = request.To,
                        };

                        await _emailRepository.Add(email);

                        messageId = email.Id.ToString();
                    }

                    await _mailHistoryRepository.Add(new Domain.Entities.CommunicationRegistry
                    {
                        Message = request.Body,
                        Title = request.Subject,
                        From = request.From ?? _options.ApiEmail,
                        To = request.To,
                        CorrelationId = messageId
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
