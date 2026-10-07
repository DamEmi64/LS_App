using Base;
using InvoiceBase.Events;
using Invoices.Domain.Entities;
using Invoices.Domain.Repositories;

namespace Invoices.Infrastructure.Connect.CreateInvoice
{
    public class CreateInvoiceMethod : ConnectMethod<CreateInvoiceEvent, CreateInvoiceValidator, CreateInvoiceResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public CreateInvoiceMethod(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public override async Task<CreateInvoiceResponse> HandleAsync(CreateInvoiceEvent request, CancellationToken cancellationToken)
        {
            var entity = new Invoice
            {
                Title = request.Title,
                Collector = request.Collector.Clone(),
                Recipient = request.Recipient.Clone(),
                Positions = request.Positions.Select(x => new InvoicePosition
                {
                    Title = x.Title,
                    Value = x.Value
                }).ToList()
            };

            await _invoiceRepository.Add(entity);

            return new CreateInvoiceResponse(entity.Id);
        }
    }
}
