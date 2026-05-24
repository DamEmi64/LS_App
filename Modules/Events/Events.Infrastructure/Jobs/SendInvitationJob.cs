using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Events.Extras.Resources;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs
{
    public class SendInvitationJob : IJob
    {
        private const string TemplatePath = "/Views/EventInvitation.cshtml";
        private const string LinkToEvent = "https://lsfamilia-app.web.app/events#{0}";

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
            if (string.IsNullOrEmpty(Receiver.Email))
            {
                await jobContext.AddError($"User {Receiver.Login} has no email");
                return;
            }

            var image = await mediaProvider.Load(Event.Image);

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new EventSendingData(Event, Receiver, image?.ContentStr, string.Format(LinkToEvent, Event.Id)));

            var sendEmailData = new SharedEvents.Communication.SendEmail(Receiver.Email, $"Invitation to event {Event.Title}", html);

            var result = await connectClient.Send(sendEmailData);

            if (result.IsFailed)
            {
                foreach (var error in result.Errors)
                {
                    await jobContext.AddError(error.Message);
                }
            }
        }
    }
}
