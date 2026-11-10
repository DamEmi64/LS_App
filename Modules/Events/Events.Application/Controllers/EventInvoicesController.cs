using Base;
using Events.Application.Dtos;
using Events.Domain.Dictionaries;
using Events.Domain.Repositories;
using InvoiceBase;
using InvoiceBase.Events;
using Microsoft.AspNetCore.Mvc;

namespace Events.Application.Controllers
{
    [AuthPermission("events")]
    public class EventInvoicesController : BaseController
    {
        private readonly IEventRepository _eventRepository;

        public EventInvoicesController(IControllerService controllerService, IEventRepository eventRepository)
            : base(controllerService) => _eventRepository = eventRepository;

        [HttpGet]
        public IActionResult List()
        {
            var users = Users.ToDictionary(x => x.UserId, x => x.Login);
            var rows = _eventRepository.GetInvoices().Select(x => new EventInvoiceDto
            {
                EventId = x.Event.Id,
                EventTitle = x.Event.Title,
                InvoiceId = x.InvoiceId,
                UserId = x.UserId,
                UserLogin = users.GetValueOrDefault(x.UserId),
                Value = x.Value
            });
            return Ok(rows);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventInvoiceDto dto)
        {
            if (CurrentUser is null) return Unauthorized();
            if (dto.Positions.Count == 0) return BadRequest(EventLogs.InvoiceDetailsRequired);

            var ev = await _eventRepository.Get(dto.EventId);
            if (ev is null) return NotFound("Event was not found.");

            var allocations = dto.Positions.SelectMany(x => x.Allocations).ToList();
            if (allocations.Count == 0)
                return BadRequest(EventLogs.InvoiceDetailsRequired);

            if (allocations.Any(x => x.Value < 0 || string.IsNullOrWhiteSpace(x.UserId)))
                return BadRequest(EventLogs.InvalidAllocation);
            if (allocations.GroupBy(x => x.UserId).Any(group => group.Count() != dto.Positions.Count(position => position.Allocations.Any(x => x.UserId == group.Key))))
                return BadRequest(EventLogs.DuplicateAllocation);
            if (allocations.Any(x => !ev.Participates.Any(p => p.UserId == x.UserId)))
                return BadRequest(EventLogs.AllocationNotParticipant);

            var recipients = Users.ToDictionary(x => x.UserId);
            foreach (var userAllocations in allocations.GroupBy(x => x.UserId))
            {
                if (!recipients.TryGetValue(userAllocations.Key, out var recipient))
                    return BadRequest(EventLogs.UserNotFound, userAllocations.Key);

                var positions = dto.Positions
                    .SelectMany(position => position.Allocations
                        .Where(allocation => allocation.UserId == userAllocations.Key)
                        .Select(allocation => new CreateinvoicePosition(position.PositionTitle, allocation.Value)))
                    .ToList();
                var totalValue = positions.Sum(position => position.Value);

                var result = await Connect.CreateInvoice($"{ev.Title}: {dto.Title}", CurrentUser, recipient, positions);

                if (result.IsFailed) return BadRequest(EventLogs.ValidationFailed, string.Join("; ", result.Errors.Select(x => x.Message)));
                await _eventRepository.AddInvoice(ev.Id, result.Value.Id, userAllocations.Key, totalValue);
            }

            return Ok();
        }
    }
}

