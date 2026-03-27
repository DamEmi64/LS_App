using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.EmailGenerator.Strategies;
using Communication.Infrastructure.Services.SendService.Models;
using Files.Domain.Repositories;
using Fluid;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;

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
            var repo = jobContext.ServiceProvider.GetRequiredService<IEmailRepository>();

            await ExecuteInternal(repo);
        }

        private async Task ExecuteInternal(IEmailRepository emailRepository)
        {
            ArgumentNullException.ThrowIfNull(Model);

            foreach (var receiver in Model.Recipients)
            {
                if (Model.Template is null || Model.Sender is null)
                    continue;

                var parser = new FluidParser(new FluidParserOptions { AllowFunctions = true });
                var result = parser.TryParse(Model.Template.Body, out var template, out var error);

                if (!result || receiver is null)
                {
                    continue;
                }

                var body = await template.RenderAsync(GenerateContext());

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

        private TemplateContext GenerateContext()
        {
            ArgumentNullException.ThrowIfNull(Model?.Sender?.Login);

            var context = new TemplateContext(Model);

            var randomStrategy = new RandomStrategy();
            var randomUniqueStrategy = new RandomUniqueStrategy();
            var incrementStrategy = new IncrementStrategy();
            var randomNumberStrategy = new RandomNumberStrategy();

            var logins = Model.Recipients.Where(x => !string.IsNullOrEmpty(x.Login)).Select(x => x.Login ?? string.Empty).ToList();

            var random = new FunctionValue((args, context) =>
            {
                return randomStrategy.Handle(args, logins, Model.Sender.Login, Model.Sender.Login);
            });

            var randomUnique = new FunctionValue((args, context) =>
            {
                return randomUniqueStrategy.Handle(args, logins, Model.Sender.Login, Model.Sender.Login);
            });

            var increment = new FunctionValue((args, context) =>
            {
                return incrementStrategy.Handle(args, logins, Model.Sender.Login, Model.Sender.Login);
            });

            var randomNumber = new FunctionValue((args, context) =>
            {
                return randomNumberStrategy.Handle(args, logins, Model.Sender.Login, Model.Sender.Login);
            });

            context.SetValue(nameof(random), random);
            context.SetValue(nameof(randomUnique), randomUnique);
            context.SetValue(nameof(increment), increment);
            context.SetValue(nameof(randomNumber), randomNumber);

            return context;
        }
    }
}