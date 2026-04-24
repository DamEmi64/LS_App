using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.EmailGenerator;
using Communication.Infrastructure.EmailGenerator.Strategies;
using Communication.Infrastructure.Services.SendService.Models;
using CommunicationBase;
using CommunicationBase.Interfaces;
using Files.Domain.Repositories;
using Fluid;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Communication.Infrastructure.Jobs
{
    public class GenEmailJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Generate email {Model?.Template?.Subject} for {string.Join(",", Model?.Recipients.Select(x => x.Email) ?? Array.Empty<string>())}";

        public EmailGenerationModel? Model { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateFromTemplate;

        public async Task Execute(IJobContext jobContext)
        {
            var repo = jobContext.Resolve<IEmailRepository>();

            await ExecuteInternal(repo, jobContext);
        }

        private async Task ExecuteInternal(IEmailRepository emailRepository, IJobContext jobContext)
        {
            ArgumentNullException.ThrowIfNull(Model);

            if (Model.Template is null || Model.Sender is null)
                return;

            var parser = jobContext.Resolve<IFluidParser>(nameof(EmailFluidParser));

            if (parser is EmailFluidParser emailParser)
            {
                emailParser.Sender = Model.Sender;
                emailParser.Receivers = Model.Recipients.ToList();

                foreach (var receiver in emailParser.Receivers)
                {
                    emailParser.Receiver = receiver;

                    var decoded = WebUtility.HtmlDecode(Model.Template.Body);
                    var body = await FluidGenerator.GenerateAsync(decoded, FluidGenerator.GenerateContext());

                    ArgumentNullException.ThrowIfNull(body);

                    var email = new Email
                    {
                        Sender = Model.Sender.Email ?? string.Empty,
                        Recipient = receiver.Email ?? string.Empty,
                        Subject = Model.Template.Subject,
                        Body = body
                    };

                    await emailRepository.Add(email);
                }
            }
        }
    }
}