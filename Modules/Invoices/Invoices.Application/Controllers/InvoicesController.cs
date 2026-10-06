using Base;
using Invoices.Application.Dtos;
using Invoices.Domain.Dictionaries;
using Invoices.Domain.Entities;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Models;
using Invoices.Infrastructure.Services.InvoiceDocumentService;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Application.Controllers
{
    [AuthPermission("invoices")]
    public class InvoicesController : BaseController
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDocumentService _invoiceDocumentService;

        public InvoicesController(IControllerService controllerService, IInvoiceRepository invoiceRepository, IInvoiceDocumentService invoiceDocumentService) : base(controllerService)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceDocumentService = invoiceDocumentService;
        }

        [HttpGet("collector")]
        public async Task<IActionResult> CollectorInvoices()
        {
            if (CurrentUser is null) return Unauthorized();
            var invoices = await _invoiceRepository.GetByCollector(CurrentUser.UserId);
            return Ok(invoices.Select(ToDto));
        }

        [HttpGet("recipient")]
        public async Task<IActionResult> RecipientInvoices()
        {
            if (CurrentUser is null) return Unauthorized();
            var invoices = await _invoiceRepository.GetByRecipient(CurrentUser.UserId);
            return Ok(invoices.Select(ToDto));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveInvoiceDto dto)
        {
            if (CurrentUser is null) return Unauthorized();
            var recipient = Users.FirstOrDefault(x => x.UserId == dto.RecipientId);
            if (recipient is null) return BadRequest("Recipient was not found.");

            var invoice = new Invoice
            {
                Title = dto.Title,
                Collector = CurrentUser.Clone(),
                Recipient = recipient.Clone(),
                Positions = dto.Positions.Select(ToEntity).ToList()
            };
            await _invoiceRepository.Add(invoice);
            return CreatedAtAction(nameof(Get), new { id = invoice.Id }, ToDto(invoice));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (!IsParticipant(invoice)) return Forbid();
            return Ok(ToDto(invoice));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] SaveInvoiceDto dto)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (invoice.Collector.UserId != CurrentUser.UserId) return Forbid();
            var recipient = Users.FirstOrDefault(x => x.UserId == dto.RecipientId);
            if (recipient is null) return BadRequest("Recipient was not found.");

            var dtoPositions = invoice.Positions.ToList();
            foreach (var position in dtoPositions)
            {
                var positionDto = dto.Positions.FirstOrDefault(x => x.Title == position.Title);
                if (positionDto is null) await _invoiceRepository.RemovePosition(position);
                position.Value = positionDto?.Value ?? 0;
            }

            var newPositions = dto.Positions.Where(x => !invoice.Positions.Any(p => p.Title == x.Title)).Select(ToEntity).ToList();

            invoice.Positions.AddRange(newPositions);

            invoice.Title = dto.Title;
            invoice.Recipient = recipient.Clone();

            await _invoiceRepository.Update(invoice);

            return Ok(ToDto(invoice));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (invoice.Collector.UserId != CurrentUser.UserId) return Forbid();
            await _invoiceRepository.Remove(invoice);
            return NoContent();
        }

        [HttpPut("{id:guid}/positions/{positionId:guid}/paid")]
        public async Task<IActionResult> MarkPositionPaid(Guid id, Guid positionId)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (invoice.Collector.UserId != CurrentUser.UserId) return Forbid();
            if (!await _invoiceRepository.MarkPositionPaid(invoice, positionId, InvoiceStatus.Paid.Key)) return NotFound();
            return Ok(ToDto(invoice));
        }

        [HttpPost("{id:guid}/document")]
        public async Task<IActionResult> GenerateDocument(Guid id, [FromBody] GenerateInvoiceDocumentDto dto)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (invoice.Collector.UserId != CurrentUser.UserId) return Forbid();
            if (dto.PaymentMethod == InvoicePaymentMethod.AccountNumber && string.IsNullOrWhiteSpace(dto.AccountNumber))
                return BadRequest("Account number is required.");
            await _invoiceDocumentService.GenerateInvoice(id, dto.PaymentMethod, CurrentUser, dto.AccountNumber);
            return Accepted(new { id });
        }

        [HttpGet("{id:guid}/document")]
        public async Task<IActionResult> DownloadDocument(Guid id)
        {
            if (CurrentUser is null) return Unauthorized();
            var invoice = await _invoiceRepository.GetWithDetails(id);
            if (invoice is null) return NotFound();
            if (!IsParticipant(invoice)) return Forbid();
            var document = await _invoiceRepository.GetDocument(id);
            if (document is null || document.Pdf.Length == 0) return NotFound();
            var fileName = string.Concat(invoice.Title.Select(character => Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));
            return File(document.Pdf, "application/pdf", $"{fileName}.pdf");
        }

        private bool IsParticipant(Invoice invoice) =>
            invoice.Collector.UserId == CurrentUser?.UserId || invoice.Recipient.UserId == CurrentUser?.UserId;

        private static InvoicePosition ToEntity(SaveInvoicePositionDto dto) => new()
        {
            Title = dto.Title ?? string.Empty,
            Value = dto.Value,
            Status = InvoiceStatus.Unpaid.Key
        };

        private static InvoiceDto ToDto(Invoice invoice) => new()
        {
            Id = invoice.Id,
            Title = invoice.Title,
            CollectorId = invoice.Collector.UserId,
            CollectorLogin = invoice.Collector.Login ?? string.Empty,
            RecipientId = invoice.Recipient.UserId,
            RecipientLogin = invoice.Recipient.Login ?? string.Empty,
            Positions = invoice.Positions.Select(x => new InvoicePositionDto
            {
                Id = x.Id,
                Title = x.Title,
                Value = x.Value,
                Status = x.Status
            }).ToList()
        };
    }
}
