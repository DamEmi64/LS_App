using Base;
using Base.Connect;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Models;
using Razor.Templating.Core;
using Invoices.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Invoices.Domain.Dictionaries;
using System.Globalization;

namespace Invoices.Infrastructure.Jobs
{
    public class GenerateInvoiceDocument
    {
        public class Job : IJob
        {
            public int OperationId => Operations.GenerateInvoice;

            public Guid Id { get; set; } = Guid.NewGuid();

            public List<IJob> Children => [];

            public string Name => $"Generate Invoice {Model.Invoice.Title}";

            public required InvoiceDocumentModel Model { get; set; }
            public bool IsPdf { get; set; }
        }

        public class Handler : JobHandler<Job>
        {
            private const string TemplatePath = "/Views/Invoice.cshtml";

            private readonly IInvoiceRepository _invoiceRepository;

            public Handler(IJobContext jobContext, IInvoiceRepository invoiceRepository) : base(jobContext)
            {
                _invoiceRepository = invoiceRepository;
            }

            public override async Task Execute(Job request)
            {
                ArgumentNullException.ThrowIfNull(request.Model);
                var invoice = request.Model.Invoice;
                var resource = new Invoices.Extras.Resources.InvoiceDocument
                {
                    Title = invoice.Title,
                    Collector = invoice.Collector.Login ?? invoice.Collector.UserId,
                    Recipient = invoice.Recipient.Login ?? invoice.Recipient.UserId,
                    Items = invoice.Positions.Where(x=>x.Status != InvoiceStatus.Paid).Select(x => new Invoices.Extras.Resources.InvoiceItem { Title = x.Title, Value = x.Value }).ToList(),
                    PaymentMethod = request.Model.PaymentMethod == InvoicePaymentMethod.BlikPhone ? "blik" : "account",
                    Phone = request.Model.CollectorPhoneNumber,
                    AccountNo = request.Model.AccountNumber
                };
                var html = await RazorTemplateEngine.RenderAsync(TemplatePath, resource);
                QuestPDF.Settings.License = LicenseType.Community;
                var polish = CultureInfo.GetCultureInfo("pl-PL");
                var pdf = Document.Create(container => container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(52);
                    page.MarginVertical(42);
                    page.DefaultTextStyle(style => style.FontFamily("Arial").FontSize(10).FontColor("#243247"));
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(header =>
                        {
                            header.Item().Text("FAKTURA").FontSize(9).Bold().FontColor("#64748B").LetterSpacing(1.5f);
                            header.Item().PaddingTop(4).Text(resource.Title).FontSize(22).Bold().FontColor("#172B4D");
                        });
                        row.ConstantItem(115).AlignRight().PaddingTop(5).Text($"Wystawiono\n{DateTime.Now.ToString("d MMMM yyyy", polish)}")
                            .FontSize(9).FontColor("#64748B");
                    });
                    page.Content().PaddingTop(24).Column(column =>
                    {
                        column.Spacing(20);
                        column.Item().Row(parties =>
                        {
                            parties.RelativeItem().PaddingRight(8).Element(card => PartyCard(card, "WYSTAWCA", resource.Collector));
                            parties.RelativeItem().PaddingLeft(8).Element(card => PartyCard(card, "ODBIORCA", resource.Recipient));
                        });
                        column.Item().Column(items =>
                        {
                            items.Spacing(8);
                            items.Item().Text("Pozycje faktury").FontSize(13).Bold().FontColor("#172B4D");
                            items.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(36);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(1.5f);
                                });
                                table.Header(header =>
                                {
                                    header.Cell().Element(TableHeader).Text("Lp.");
                                    header.Cell().Element(TableHeader).Text("Opis");
                                    header.Cell().Element(TableHeader).AlignRight().Text("Kwota");
                                });
                                for (var index = 0; index < resource.Items.Count; index++)
                                {
                                    var item = resource.Items[index];
                                    var background = index % 2 == 0 ? "#FFFFFF" : "#F8FAFC";
                                    table.Cell().Background(background).BorderBottom(1).BorderColor("#E2E8F0").Padding(9).Text((index + 1).ToString());
                                    table.Cell().Background(background).BorderBottom(1).BorderColor("#E2E8F0").Padding(9).Text(item.Title);
                                    table.Cell().Background(background).BorderBottom(1).BorderColor("#E2E8F0").Padding(9).AlignRight().Text(item.Value.ToString("C", polish));
                                }
                            });
                            items.Item().AlignRight().PaddingTop(5).Text(text =>
                            {
                                text.Span("Razem   ").FontSize(12).Bold().FontColor("#475569");
                                text.Span(resource.Items.Sum(item => item.Value).ToString("C", polish)).FontSize(17).Bold().FontColor("#172B4D");
                            });
                        });
                        column.Item().Element(payment => PaymentCard(payment, resource.PaymentInfo));
                    });
                    page.Footer().PaddingTop(10).BorderTop(1).BorderColor("#E2E8F0").Row(footer =>
                    {
                        footer.RelativeItem().Text("Dziękujemy za współpracę").FontSize(8).FontColor("#64748B");
                        footer.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("Strona ").FontSize(8).FontColor("#64748B");
                            text.CurrentPageNumber();
                            text.Span(" z ").FontSize(8).FontColor("#64748B");
                            text.TotalPages();
                        });
                    });
                })).GeneratePdf();

                var invoiceDb = await _invoiceRepository.GetWithDetails(invoice.Id);
                ArgumentNullException.ThrowIfNull(invoiceDb);

                await _invoiceRepository.SaveDocument(new InvoiceDocument { Invoice = invoiceDb, Html = html, Pdf = pdf });
            }

            private static IContainer TableHeader(IContainer container) => container
                .Background("#172B4D").Padding(9).DefaultTextStyle(style => style.FontColor("#FFFFFF").Bold().FontSize(9));

            private static void PartyCard(IContainer container, string label, string name) => container
                .Background("#F1F5F9").Border(1).BorderColor("#E2E8F0").Padding(14).Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text(label).FontSize(8).Bold().FontColor("#64748B").LetterSpacing(1);
                    column.Item().Text(name).FontSize(11).SemiBold().FontColor("#172B4D");
                });

            private static void PaymentCard(IContainer container, string paymentInfo) => container
                .Background("#EFF6FF").BorderLeft(3).BorderColor("#2563EB").Padding(14).Column(column =>
                {
                    column.Spacing(4);
                    column.Item().Text("INFORMACJE O PŁATNOŚCI").FontSize(8).Bold().FontColor("#1D4ED8").LetterSpacing(1);
                    column.Item().Text(paymentInfo).FontSize(10).FontColor("#1E293B");
                });

        }
    }
}
