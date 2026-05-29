using Base;
using CommunicationBase;
using Events.Domain;
using Events.Extras.Resources;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs.SendInvitation
{
    public class SendInvitationJobHandler : JobHandler<SendInvitationJob>
    {
        private const string TemplatePath = "/Views/EventInvitation.cshtml";

        private readonly IMediaProvider _mediaProvider;
        private readonly IConnect _connectClient;
        private readonly string _linkToEventTemplate;

        public SendInvitationJobHandler(IJobContext jobContext,
            IMediaProvider mediaProvider,
            IOptions<EventOptions> options,
            IConnect connectClient)
            : base(jobContext)
        {
            _mediaProvider = mediaProvider;
            _connectClient = connectClient;
            _linkToEventTemplate = options.Value.EventLinkTemplate;
        }

        public override async Task Execute(SendInvitationJob request)
        {
            if (string.IsNullOrEmpty(request.Receiver.Email))
            {
                await LogError($"User {request.Receiver.Login} has no email");
                return;
            }

            var image = await _mediaProvider.Load(request.Event.Image);

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new EventSendingData(request.Event, request.Receiver, image?.ContentStr, string.Format(_linkToEventTemplate, request.Event.Id)));

            var result = await _connectClient.SendEmailAsync(request.Receiver.Email, $"Invitation to event {request.Event.Title}", html);

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
