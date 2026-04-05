using Base;
using Communication.Application.Dtos;
using Communication.Application.Filters;
using Communication.Domain.Entities;
using Communication.Infrastructure.Services.SendService;
using Communication.Infrastructure.Services.SendService.Models;
using Files.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Application.Controllers
{
    [Route("[controller]")]
    [AuthPermission("communication")]
    public class TemplatesController : BaseController
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ISendService _sendService;

        public TemplatesController(
            IControllerService controllerService,
            ITemplateRepository templateRepository,
            ISendService sendService)
            : base(controllerService)
        {
            _templateRepository = templateRepository;
            _sendService = sendService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _templateRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        public IActionResult ListData([FromQuery] TemplateFilter filter)
        {
            return Json(filter.Filter(_templateRepository.GetAll()));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] Template template)
        {
            await _templateRepository.Add(template);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Template data)
        {
            var template = await _templateRepository.Get(id);

            ArgumentNullException.ThrowIfNull(template);

            template.Subject = data.Subject;
            template.Body = data.Body;
            template.UpdDate = DateTimeOffset.Now;

            await _templateRepository.Update(template);

            return Ok();
        }

        [HttpPut("{id}/generate")]
        public async Task<IActionResult> Send(Guid id,[FromBody] EmailGenerationDto dto)
        {
            var template = await _templateRepository.Get(dto.Template ?? id);

            if (template is null)
            {
                return NotFound();
            }

            var model = new EmailGenerationModel
            {
                Template = template,
                Sender = dto.Sender,
                Recipients = dto.Recipients
            };

            var processTitle = await _sendService.GenerateFromTemplate(model, await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(NotifyTypes.ProcessQueued, processTitle);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _templateRepository.Remove(id);

            return Ok();
        }
    }
}