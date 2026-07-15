using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Application.Controllers
{
    [AuthPermission("communication")]
    [AuthPermission("communication-discord")]
    public class DiscordController : BaseController
    {
        private readonly IDiscordRepository _discordRepository;

        public DiscordController(IControllerService controllerService, IDiscordRepository discordRepository) : base(controllerService)
        {
            _discordRepository = discordRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _discordRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<DiscordCmd>), StatusCodes.Status200OK)]
        public IActionResult ListData()
        {
            var discordCmds = _discordRepository.GetAll().ToList();

            return Json(discordCmds, discordCmds.Count);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] DiscordCmd discordData)
        {
            var discordCmd = await _discordRepository.Get(id);

            ArgumentNullException.ThrowIfNull(discordCmd, nameof(discordCmd));

            discordCmd.Response = discordData.Response;
            discordCmd.Active = discordData.Active;

            await _discordRepository.Update(discordCmd);

            return Ok();
        }
    }
}
