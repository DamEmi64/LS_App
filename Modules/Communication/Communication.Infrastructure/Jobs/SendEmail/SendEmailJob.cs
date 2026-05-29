using Base;
using Communication.Domain.Entities;

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
    }
}