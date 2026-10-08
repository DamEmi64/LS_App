using FluentValidation;
using InvoiceBase.Events;

namespace Invoices.Infrastructure.Connect.CreateInvoice
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceEvent>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Recipient).NotNull();
            RuleFor(x => x.Collector).NotNull();
            RuleForEach(x => x.Positions).SetValidator(x => new CreateInvoicePositionValidator());
        }

        public class CreateInvoicePositionValidator: AbstractValidator<CreateinvoicePosition>
        {
            public CreateInvoicePositionValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
            }
        }
    }
}
