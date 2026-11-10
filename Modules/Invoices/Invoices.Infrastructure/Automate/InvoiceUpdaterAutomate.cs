using Base;
using Invoices.Domain.Dictionaries;
using Invoices.Domain.Events;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Jobs;
using Invoices.Infrastructure.Models;

namespace Invoices.Infrastructure.Automate
{
    public class InvoiceUpdaterAutomate : AutomationBase.SystemAutomate
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceUpdaterAutomate(IJobEngine jobEngine, IInvoiceRepository invoiceRepository) : base(jobEngine)
        {
            _invoiceRepository = invoiceRepository;
        }

        public override int Key => InvoiceAutomats.UpsertInvoiceDocument;

        public override bool CanExecute(INotifyEvent @event) => @event is InvoiceCreateOrUpdateEvent;

        public override async Task Execute(INotifyEvent @event, string configuration)
        {
            if (@event is InvoiceCreateOrUpdateEvent invoiceEvent)
            {
                var invoice = await _invoiceRepository.GetWithDetails(invoiceEvent.InvoiceId);
                if (invoice != null)
                {
                    var schema = CreateJobSchema($"Update Invoice Document: {invoice.Title}");
                    schema.AddJob(new GenerateInvoiceDocument.Job
                    {
                        Model = new InvoiceDocumentModel
                        {
                            Invoice = invoice,
                            CollectorPhoneNumber = invoice.Collector?.Phone,
                            PaymentMethod = InvoicePaymentMethod.None,
                        },
                    });
                }
            }
        }
    }
}
