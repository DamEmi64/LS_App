using Base;
using Events.Extras.Resources;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs.SendInvitation
{
    public class SendInvitationJobHandler : JobHandler<SendInvitationJob>
    {
        private const string TemplatePath = "/Views/EventInvitation.cshtml";
        private const string LinkToEvent = "https://lsfamilia-app.web.app/events#{0}";

        private readonly IMediaProvider _mediaProvider;
        private readonly IConnect _connectClient;

        public SendInvitationJobHandler(IJobContext jobContext,
            IMediaProvider mediaProvider,
            IConnect connectClient)
            : base(jobContext)
        {
            _mediaProvider = mediaProvider;
            _connectClient = connectClient;
        }

        public override async Task Execute(SendInvitationJob request)
        {
            if (string.IsNullOrEmpty(request.Receiver.Email))
            {
                await LogError($"User {request.Receiver.Login} has no email");
                return;
            }

            var image = await _mediaProvider.Load(request.Event.Image);

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new EventSendingData(request.Event, request.Receiver, image?.ContentStr, string.Format(LinkToEvent, request.Event.Id)));

            var sendEmailData = new SharedEvents.Communication.SendEmail(request.Receiver.Email, $"Invitation to event {request.Event.Title}", html);

            var result = await _connectClient.Send(sendEmailData);

            if (result.IsFailed)
            {
                foreach (var error in result.Errors)
                {
                    await LogError(error.Message);
                }
            }
        }
    }
}
