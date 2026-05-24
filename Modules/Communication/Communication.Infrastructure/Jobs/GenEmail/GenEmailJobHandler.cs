using Base;
using Base.Job;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.EmailGenerator;
using Communication.Infrastructure.Repositories;
using CommunicationBase;
using CommunicationBase.Interfaces;
using Files.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Communication.Infrastructure.Jobs.GenEmail
{
    public class GenEmailJobHandler : JobHandler<GenEmailJob>
    {
        private readonly IFluidParser _fluidParser;
        private readonly IEmailRepository _emailRepository;

        public GenEmailJobHandler(IJobContext jobContext,
            [FromKeyedServices(nameof(EmailFluidParser))] IFluidParser fluidParser,
            IEmailRepository emailRepository) 
            : base(jobContext)
        {
            _fluidParser = fluidParser;
            _emailRepository = emailRepository;
        }

        public override async Task Execute(GenEmailJob request)
        {
            ArgumentNullException.ThrowIfNull(request.Model);

            if (request.Model.Template is null || request.Model.Sender is null)
                return;


            if (_fluidParser is EmailFluidParser emailParser)
            {
                emailParser.Sender = request.Model.Sender;
                emailParser.Receivers = request.Model.Recipients.ToList();

                foreach (var receiver in emailParser.Receivers)
                {
                    emailParser.Receiver = receiver;

                    var decoded = WebUtility.HtmlDecode(request.Model.Template.Body);
                    var body = await FluidGenerator.GenerateAsync(decoded, FluidGenerator.GenerateContext());

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
}
