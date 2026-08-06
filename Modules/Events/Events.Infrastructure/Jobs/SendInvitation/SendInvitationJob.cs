using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;

namespace Events.Infrastructure.Jobs
{
    public class SendInvitationJob : IJob
    {
        public int OperationId => Operations.SendInvitation;

        public Guid Id { get; set; }

        public List<IJob> Children => new();

        public string Name => $"Send invitation about {Event?.Title}";

        public required Event Event { get; set; }

        public required UserData Receiver { get; set; }
    }
}
