using AutoMapper;
using Base;
using Communication.Application.Dtos;
using Communication.Application.Filters;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Services;
using Communication.Infrastructure.Services.SendService;
using Communication.Infrastructure.Services.SendService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Application.Controllers
{
    [AuthPermission("communication")]
    public class TemplatesController : BaseController
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ISendService _sendService;
        private readonly IFluidService _fluidService;
        private readonly IMapper _mapper;

        public TemplatesController(
            IControllerService controllerService,
            ITemplateRepository templateRepository,
            ISendService sendService,
            IFluidService fluidService,
            IMapper mapper)
            : base(controllerService)
        {
            _templateRepository = templateRepository;
            _sendService = sendService;
            _fluidService = fluidService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Template), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _templateRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("rules")]
        [ProducesResponseType(typeof(RulesDto), StatusCodes.Status200OK)]
        public IActionResult Rules([FromQuery] TemplateFilter filter)
        {
            var functions = _fluidService.GetFunctions();
            var variables = _fluidService.GetVariables();

            return Json(new RulesDto
            {
                Functions = functions.Select(x => _mapper.Map<FluidDto>(x)).ToList(),
                Variables = variables.Select(x => new FluidDto
                {
                    Id = int.TryParse(x.Key, out var id) ? id : Random.Shared.Next(),
                    Invoker = x.Key,
                    Title = x.Key,
                }).ToList()
            });
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<Template>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] TemplateFilter filter)
        {
            return Json(filter.Filter(_templateRepository.GetAll(), out var count), count);
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
        public async Task<IActionResult> Send(Guid id, [FromBody] EmailGenerationDto dto)
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

            var processTitle = await _sendService.GenerateFromTemplate(model, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
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