using Automation.Application.Filters;
using Automation.Domain.Entities;
using Automation.Domain.Repositories;
using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Automation.Application.Controllers
{
    [AuthPermission("automation")]
    public class SystemAutomateController : BaseController
    {
        private readonly ISystemAutomateRepository _automatRepository;

        public SystemAutomateController(
            IControllerService controllerService,
            ISystemAutomateRepository automatRepository)
            : base(controllerService)
        {
            _automatRepository = automatRepository;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<SystemAutomat>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListData([FromQuery] SystemAutomateFilter filter)
        {
            var automats = _automatRepository.GetAll();
            return Json(filter.Filter(automats, out var count), count);
        }

        [HttpPut("{id}/configuration")]
        public async Task<IActionResult> EditConfiguration(Guid id, [FromBody] string configuration)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);

            automat.Configuration = configuration;

            await _automatRepository.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);

            automat.IsActive = true;

            await _automatRepository.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var automat = await _automatRepository.Get(id);
            ArgumentNullException.ThrowIfNull(automat);

            automat.IsActive = false;

            await _automatRepository.SaveChanges();

            return Ok();
        }
    }
}
