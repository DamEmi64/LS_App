using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.EmailGenerator;
using CommunicationBase;
using CommunicationBase.Interfaces;
using System.Net;

namespace Communication.Infrastructure.Jobs.GenEmail
{
    public class GenEmailJobHandler : JobHandler<GenEmailJob>
    {
        private readonly List<IFluidParser> _fluidParsers;
        private readonly IEmailRepository _emailRepository;

        public GenEmailJobHandler(IJobContext jobContext,
            IEmailRepository emailRepository,
            IEnumerable<IFluidParser> fluidParsers)
            : base(jobContext)
        {
            _emailRepository = emailRepository;
            _fluidParsers = fluidParsers.ToList();
        }

        public override async Task Execute(GenEmailJob request)
        {
            ArgumentNullException.ThrowIfNull(request.Model);

            if (request.Model.Template is null || request.Model.Sender is null)
                return;

            var emailParser = new EmailFluidParser
            {
                Sender = EmailUserData.Parse(request.Model.Sender),
                Receivers = request.Model.Recipients.Select(EmailUserData.Parse).ToList()
            };

            var parsers = _fluidParsers.Where(x => x is not EmailFluidParser).ToList();
            parsers.Add(emailParser);

            foreach (var receiver in emailParser.Receivers)
            {
                emailParser.Receiver = receiver;
                var a = emailParser.Functions;
                var decoded = WebUtility.HtmlDecode(request.Model.Template.Body);
                var body = await FluidGenerator.GenerateAsync(decoded, FluidGenerator.GenerateContext(parsers));

                ArgumentNullException.ThrowIfNull(body);

                var email = new Email
                {
                    Sender = request.Model.Sender.Email ?? string.Empty,
                    Recipient = receiver.Email ?? string.Empty,
                    Subject = request.Model.Template.Subject,
                    Body = body
                };

                await _emailRepository.Add(email);
            }

        }
    }
}
