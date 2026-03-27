using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.Jobs;
using Communication.Infrastructure.Services.SendService.Models;

namespace Communication.Infrastructure.Services.SendService
{
    public class SendService : ISendService
    {
        private readonly IJobEngine _jobEngine;

        public SendService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task<string> SendMail(IEnumerable<Email> emails, UserData userData)
        {
            var title = $"Sending emails to {string.Join(",", emails.Select(x => x.Recipient))}";
            var schema = _jobEngine.Create(title);

            foreach (var email in emails)
            {
                schema.AddJob(new SendEmailJob
                {
                    Email = email
                });
            }

            await _jobEngine.Execute(schema, userData);

            return title;
        }

        public async Task<string> GenerateFromTemplate(EmailGenerationModel model, UserData userData)
        {
            var title = $"Generate emails for {string.Join(",", model.Recipients.Select(x => x.Login))}";
            var schema = _jobEngine.Create(title)
                                .AddJob(new GenEmailJob
                                {
                                    Model = model
                                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }
    }
}