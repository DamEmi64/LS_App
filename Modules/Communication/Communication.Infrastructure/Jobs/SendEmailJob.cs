using Base;
using Communication.Domain.Entities;
using CommunicationBase;
using Files.Domain.Repositories;

namespace Communication.Infrastructure.Jobs
{
    public class SendEmailJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Send mail to {Email?.Recipient}";

        public Email? Email { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.SendEmail;

        public async Task Execute(IJobContext jobContext)
        {
            var repo = jobContext.Resolve<IEmailRepository>();
            var sender = jobContext.Resolve<IConnect>();

            await ExecuteInternal(repo, sender);
        }

        private async Task ExecuteInternal(IEmailRepository emailRepository, IConnect connectClient)
        {
            ArgumentNullException.ThrowIfNull(Email);

            var result = await connectClient.SendEmailAsync(Email.Recipient, Email.Subject, Email.Body, Email.Sender);

            if (result.IsFailed)
            {
                throw new InvalidOperationException($"Failed to send email to {Email.Recipient}: {string.Join(", ", result.Errors.Select(e => e.Message))}");
            }

            var email = await emailRepository.Get(Email.Id);

            ArgumentNullException.ThrowIfNull(email, $"Email with ID {Email.Id} not found in repository.");

            email.SentDate = DateTimeOffset.Now;
            await emailRepository.Update(email);
        }
    }
}