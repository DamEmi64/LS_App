using Base;
using Invoices.Domain.Dictionaries;
using Invoices.Domain.Repositories;
using SharedEvents;

namespace Invoices.Infrastructure.Jobs
{
    public class SendInvoice
    {
        public class Job : IJob
        {
            public int OperationId => Operations.SendInvoice;

            public Guid Id { get; set; } = Guid.NewGuid();

            public List<IJob> Children => [];

            public string Name => $"Send Invoice {Title}";

            public Guid InvoiceId { get; set; }
            public required string Title { get; set; }
            public required string RecipientEmail { get; set; }
        }

        public class Handler : JobHandler<Job>
        {
            private readonly IInvoiceRepository _invoiceRepository;
            private readonly IConnect _connect;

            public Handler(IJobContext jobContext, IInvoiceRepository invoiceRepository, IConnect connect) : base(jobContext)
            {
                _invoiceRepository = invoiceRepository;
                _connect = connect;
            }
            public override async Task Execute(Job request)
            {
                var invoice = await _invoiceRepository.GetWithDetails(request.InvoiceId);
                var document = await _invoiceRepository.GetDocument(request.InvoiceId);
                if (invoice is null || document is null)
                    throw new NullReferenceException(nameof(invoice));

                if (invoice.Collector.Email is null)
                    return;

                await _connect.Send<SendEmail>(new SendEmail
                (
                    To: invoice.Collector.Email,
                    Subject: $"F: {invoice.Title}",
                    Body: document.Html ?? string.Empty,
                    From: invoice.Recipient.Email
                ));
            }
        }
    }
}
