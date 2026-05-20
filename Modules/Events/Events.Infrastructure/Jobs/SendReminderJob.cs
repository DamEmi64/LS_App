using Base;
using Base.Interfaces;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs
{
    public class SendReminderJob : IJob
    {
        private const string TemplatePath = "/Views/EventReminder.cshtml";

        public int OperationId => Operations.SendReminder;

        public Guid Id { get; set; }

        public List<IJob> Children => new List<IJob>();

        public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;

        public string Name => $"Send reminder about {Event?.Title} in {RequestDate}";

        public required Event Event { get; set; }
        public required UserData Receiver { get; set; }

        public async Task Execute(IJobContext jobContext)
        {
            var emailSender = jobContext.Resolve<IEmailSender>();
            await ExecuteInternal(emailSender, jobContext);
        }

        private async Task ExecuteInternal(IEmailSender emailSender, IJobContext jobContext)
        {
            if (string.IsNullOrEmpty(Receiver.Email))
            {
                await jobContext.AddError($"User {Receiver.Id} doesn't have email");
                return;
            }

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new { Event, Receiver });
            var result = await emailSender.SendEmailAsync(Receiver.Email, $"Reminder to event {Event.Title}", html);

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
