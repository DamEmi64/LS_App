using Base;
using Communication.Domain.Repositories;
using CommunicationBase;

namespace Communication.Infrastructure.Jobs.SendEmail
{
    public class SendEmailJobHandler : JobHandler<SendEmailJob>
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IConnect _connect;

        public SendEmailJobHandler(IJobContext jobContext,
            IEmailRepository emailRepository,
            IConnect connect)
            : base(jobContext)
        {
            _emailRepository = emailRepository;
            _connect = connect;
        }

        public override async Task Execute(SendEmailJob request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);

            var result = await _connect.SendEmailAsync(request.Email.Recipient, request.Email.Subject, request.Email.Body, request.Email.Sender);

            if (result.IsFailed)
            {
                throw new InvalidOperationException($"Failed to send email to {request.Email.Recipient}: {string.Join(", ", result.Errors.Select(e => e.Message))}");
            }

            var email = await _emailRepository.Get(request.Email.Id);

            ArgumentNullException.ThrowIfNull(email, $"Email with ID {request.Email.Id} not found in repository.");

            email.SentDate = DateTimeOffset.Now;
            await _emailRepository.Update(email);
        }
    }
}
