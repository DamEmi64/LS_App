using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Events.Extras.Resources;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs
{
    public class SendInvitationJob : IJob
    {
       public int OperationId => Operations.SendInvitation;

        public Guid Id { get; set; }

        public List<IJob> Children => new List<IJob>();

        public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;

        public string Name => $"Send invitation about {Event?.Title} {RequestDate}";

        public required Event Event { get; set; }

        public required UserData Receiver { get; set; }

        public async Task Execute(IJobContext jobContext)
        {
            var mediaProvider = jobContext.Resolve<IMediaProvider>();
            var connectClient = jobContext.Resolve<IConnect>();
            await ExecuteInternal(jobContext, mediaProvider, connectClient);
        }

        private async Task ExecuteInternal(IJobContext jobContext, IMediaProvider mediaProvider, IConnect connectClient)
        {
            
        }
    }
}
