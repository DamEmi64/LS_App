using AutoMapper;
using Base;
using Events.Application.Dtos;
using Events.Application.Filters;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Events.Domain.Repositories;
using Events.Infrastructure.Services.InvitationService;
using Events.Infrastructure.Services.ReminderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events.Application.Controllers
{
    [AuthPermission("events")]
    public class EventsController : BaseController
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IInvitationService _invitationService;
        private readonly IReminderService _reminderService;

        public EventsController(
            IControllerService controllerService,
            IEventRepository eventRepository,
            IMapper mapper,
            IInvitationService invitationService,
            IReminderService reminderService)
            : base(controllerService)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _invitationService = invitationService;
            _reminderService = reminderService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _eventRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<EventDto>(result);
            return Json(dto);
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(ResponseList<EventDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MyEvents([FromQuery] EventFilter filter)
        {
            var events = _eventRepository.GetByUser(CurrentUser?.UserId ?? string.Empty);
            return Json(filter.Filter(events, out var count), count);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<EventDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListData([FromQuery] EventFilter filter)
        {
            var events = _eventRepository.GetAll();
            return Json(filter.Filter(events, out var count), count);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] EventDto dto)
        {
            var entity = _mapper.Map<Event>(dto);
            await _eventRepository.Add(entity);

            await Notifier.Success(EventNotifyTypes.EventCreated, entity.Title);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EventDto dto)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.EventDate = dto.EventDate;

            await _eventRepository.Update(entity);
            await Notifier.Success(EventNotifyTypes.EventUpdated, dto.Title);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _eventRepository.Remove(id);
            await Notifier.Success(EventNotifyTypes.EventDeleted, id);

            return Ok();
        }

        [HttpPut("{id}/signIn")]
        public async Task<IActionResult> SignIn(Guid id)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(CurrentUser);

            var eventUser = new EventUser
            {
                Event = entity,
                UserId = CurrentUser.UserId,
                Email = CurrentUser.Email,
                Login = CurrentUser.Login
            };

            await _eventRepository.SignIn(eventUser);

            await Notifier.Success(EventNotifyTypes.EventSignIn, entity.Title);
            return Ok();
        }

        [HttpPut("{id}/signOut")]
        public async Task<IActionResult> SignOut(Guid id)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(CurrentUser);

            entity.Participates.RemoveAll(x => x.UserId == CurrentUser?.UserId);
            await _eventRepository.Update(entity);

            await Notifier.Success(EventNotifyTypes.EventSignOut, entity.Title);
            return Ok();
        }

        [HttpPost("{id}/invitation")]
        public async Task<IActionResult> SendInvitation(Guid id)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(CurrentUser);
            await _invitationService.SendInvitation(entity, Users, CurrentUser);
            await Notifier.Success(EventNotifyTypes.SendInvitation, entity.Title);
            return Ok();
        }

        [HttpPost("{id}/reminder")]
        public async Task<IActionResult> SetReminder(Guid id, [FromBody] ReminderDto dto)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(CurrentUser);
            ArgumentNullException.ThrowIfNull(entity);
            await _reminderService.AddReminder(dto.ReminderDate, entity, CurrentUser);
            await Notifier.Success(EventNotifyTypes.SetReminder, entity.Title);
            return Ok();
        }

        [HttpDelete("{id}/reminder")]
        public async Task<IActionResult> RemoveReminder(Guid id)
        {
            var entity = await _eventRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);
            await _reminderService.RemoveReminder(entity);

            await Notifier.Success(EventNotifyTypes.RemoveReminder, entity.Title);
            return Ok();
        }
    }
}