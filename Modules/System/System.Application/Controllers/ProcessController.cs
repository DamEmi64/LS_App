using AutoMapper;
using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Application.Dtos;
using System.Application.Filters;
using System.Domain.Dictionaries;
using System.Domain.Repositories;

namespace System.Application.Controllers
{
    public class ProcessController : BaseController
    {
        private readonly IProcessRepository _processRepository;
        private readonly IJobEngine _jobEngine;
        private readonly IMapper _mapper;

        public ProcessController(IControllerService controllerService,
            IProcessRepository processRepository,
            IJobEngine jobEngine,
            IMapper mapper) : base(controllerService)
        {
            _processRepository = processRepository;
            _jobEngine = jobEngine;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProcessDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _processRepository.GetReadData(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<ProcessDto>(result));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<ProcessDto>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] ProcessFilter filter)
        {
            return Json(filter.Filter(_processRepository.GetAllReadData(), out var count).Select(_mapper.Map<ProcessDto>), count);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _jobEngine.Cancel(id);
            await Notifier.Info(SystemNotifyTypes.ProcessCancelled, id);
            return Ok();
        }
    }
}