using AutoMapper;
using Automation.Application.Dtos;
using Automation.Application.Filters;
using Automation.Domain.Dictionaries;
using Automation.Domain.Entities;
using Automation.Domain.Repositories;
using Automation.Infrastructure.Services.AutomationService;
using Base;
using Microsoft.AspNetCore.Mvc;

namespace Automation.Application.Controllers
{
    [Route("[controller]")]
    [AuthPermission("automation")]
    public class AutomationsController : BaseController
    {
        private readonly IAutomatRepository _automatRepository;
        private readonly IAutomationService _automationService;
        private readonly IMapper _mapper;

        public AutomationsController(
            IControllerService controllerService,
            IAutomatRepository automatRepository,
            IAutomationService automationService,
            IMapper mapper)
            : base(controllerService)
        {
            _automatRepository = automatRepository;
            _automationService = automationService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _automatRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> Tasks(Guid id)
        {
            var result = await _automatRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result.Tasks);
        }

        [HttpGet("")]
        public async Task<IActionResult> ListData([FromQuery] AutomatonFilter filter)
        {
            var automats = _automatRepository.GetAll();
            return Json(filter.Filter(automats));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] AutomatonDto dto)
        {
            var entity = _mapper.Map<Automat>(dto);

            await _automationService.AddOrUpdateAutomat(entity);
            await Notifier.Success(AutomatNotifyTypes.AutomatCreated, dto.Title);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] AutomatonDto dto)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);

            automat.Title = dto.Title;
            automat.Description = dto.Description;
            UpdateTasks(dto.Tasks, automat);

            await _automationService.AddOrUpdateAutomat(automat);
            await Notifier.Success(AutomatNotifyTypes.AutomatUpdated, dto.Title);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);
            await _automationService.RemoveAutomat(automat);
            await Notifier.Success(AutomatNotifyTypes.AutomatDeleted, id);

            return Ok();
        }

        [HttpPut("{id}/turnoff")]
        public async Task<IActionResult> TurnOff(Guid id)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);
            await _automationService.TurnOffAutomat(automat);
            await Notifier.Success(AutomatNotifyTypes.AutomatTurnedOff, automat.Title);
            return Ok();
        }

        [HttpPut("{id}/turnon")]
        public async Task<IActionResult> TurnOn(Guid id)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);
            await _automationService.TurnOnAutomat(automat);
            await Notifier.Success(AutomatNotifyTypes.AutomatTurnedOn, automat.Title);
            return Ok();
        }

        private void UpdateTasks(List<TaskDto> tasks, Automation.Domain.Entities.Automat automat)
        {
            foreach (var task in tasks)
            {
                var existingTask = automat.Tasks.FirstOrDefault(t => t.Order == task.Order);
                if (existingTask != null)
                {
                    existingTask.OperationId = task.OperationId;
                }
                else
                {
                    automat.Tasks.Add(new Automation.Domain.Entities.Task
                    {
                        OperationId = task.OperationId,
                        Order = task.Order
                    });
                }
            }
        }
    }
}