using Base;
using CommunicationBase;
using Events.Domain;
using Events.Domain.Entities;
using Events.Extras.Resources;
using MediatR;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs.SendReminder
{
    public class SendReminderJobHandler : JobHandler<SendReminderJob>
    {
        private const string TemplatePath = "/Views/EventReminder.cshtml";

        private readonly IMediaProvider _mediaProvider;
        private readonly IConnect _connectClient;
        private readonly string _linkToEventTemplate;

        public SendReminderJobHandler(IJobContext jobContext,
            IMediaProviderFactory mediaProviderFactory,
            IOptions<EventOptions> options,
            IConnect connectClient)
            : base(jobContext)
        {
            _mediaProvider = mediaProviderFactory.Create();
            _connectClient = connectClient;
            _linkToEventTemplate = options.Value.EventLinkTemplate;
        }

        public override async Task Execute(SendReminderJob request)
        {
            foreach (var participant in request.Event.Participates)
            {
                await SendToUser(request, participant);
            }
        }

        private async Task SendToUser(SendReminderJob request, EventUser user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                await LogError($"User {user.Login} has no email");
                return;
            }

            var userData = new UserData
            {
                Email = user.Email,
                Login = user.Login,
                UserId = user.UserId
            };

            var image = await _mediaProvider.Load(request.Event.Image);

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new EventSendingData(request.Event, userData, image?.ContentStr, string.Format(_linkToEventTemplate, request.Event.Id)));

            var result = await _connectClient.SendEmailAsync(user.Email, $"Reminder for event {request.Event.Title}", html);

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
