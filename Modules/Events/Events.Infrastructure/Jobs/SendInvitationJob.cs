using Base;
using Base.Interfaces;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Razor.Templating.Core;

namespace Events.Infrastructure.Jobs
{
    public class SendInvitationJob : IJob
    {
        private const string TemplatePath = "/Views/EventInvitation.cshtml";

        public int OperationId => Operations.SendInvitation;

        public Guid Id { get; set; }

        public List<IJob> Children => new List<IJob>();

        public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;

        public string Name => $"Send invitation about {Event?.Title} {RequestDate}";

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
                await jobContext.AddError($"User {Receiver.Login} has no email");
                return;
            }

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, new { Event, Receiver });
            var result = await emailSender.SendEmailAsync(Receiver.Email, $"Invitation to event {Event.Title}", html);

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
