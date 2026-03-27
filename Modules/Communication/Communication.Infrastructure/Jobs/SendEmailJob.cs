using Base;
using Base.Interfaces;
using Communication.Domain.Entities;
using Files.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

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
            var repo = jobContext.ServiceProvider.GetRequiredService<IEmailRepository>();
            var sender = jobContext.ServiceProvider.GetRequiredService<IEmailSender>();

            await ExecuteInternal(repo, sender);
        }

        private async Task ExecuteInternal(IEmailRepository emailRepository, IEmailSender sender)
        {
            ArgumentNullException.ThrowIfNull(Email);

            var result = await sender.SendEmailAsync(Email.Recipient, Email.Subject, Email.Body, Email.Sender);

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