using Base;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Jobs;
using Invoices.Infrastructure.Models;

namespace Invoices.Infrastructure.Services.InvoiceDocumentService
{
    public sealed class InvoiceDocumentService : IInvoiceDocumentService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IJobEngine _jobEngine;

        public InvoiceDocumentService(IInvoiceRepository invoiceRepository, IJobEngine jobEngine)
        {
            _invoiceRepository = invoiceRepository;
            _jobEngine = jobEngine;
        }

        public async Task GenerateAndSendInvoice(Guid invoiceId, InvoicePaymentMethod paymentMethod, UserData collector, string? accountNo = null)
        {
            var invoice = await _invoiceRepository.GetWithDetails(invoiceId);
            ArgumentNullException.ThrowIfNull(invoice);

            var schema = _jobEngine.Create($"Generate and send invoice: {invoice.Title}");
            schema.AddJob(new GenerateInvoiceDocument.Job
            {
                Model = new InvoiceDocumentModel
                {
                    Invoice = invoice,
                    AccountNumber = accountNo,
                    CollectorPhoneNumber = collector.Phone,
                    PaymentMethod = paymentMethod
                }
            }).AddChildJob(new SendInvoice.Job
            {
                InvoiceId = invoice.Id,
                RecipientEmail = invoice.Recipient.Email ?? string.Empty,
                Title = invoice.Title
            });

            await _jobEngine.Execute(schema, collector);
        }

        public async Task GenerateInvoice(Guid invoiceId, InvoicePaymentMethod paymentMethod, UserData collector, string? accountNo = null)
        {
            var invoice = await _invoiceRepository.GetWithDetails(invoiceId);
            ArgumentNullException.ThrowIfNull(invoice);

            var schema = _jobEngine.Create($"Generate invoice: {invoice.Title}");
            schema.AddJob(new GenerateInvoiceDocument.Job
            {
                Model = new InvoiceDocumentModel
                {
                    Invoice = invoice,
                    AccountNumber = accountNo,
                    CollectorPhoneNumber = collector.Phone,
                    PaymentMethod = paymentMethod
                }
            });

            await _jobEngine.Execute(schema, collector);
        }
    }
}
