using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Events.Extras.Resources;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs
{
    public class SendReminderJob : IJob
    {
        private const string TemplatePath = "/Views/EventReminder.cshtml";
        private const string LinkToEvent = "https://lsfamilia-app.web.app/events#{0}";

        public int OperationId => Operations.SendReminder;

        public Guid Id { get; set; }

        public List<IJob> Children => new List<IJob>();

        public string Name => $"Send reminder about {Event?.Title}";

        public required Event Event { get; set; }
        public required UserData Receiver { get; set; }
    }
}
