using AutoMapper;
using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Application.Dtos;
using System.Application.Filters;
using System.Domain.Repositories;
using System.Infrastructure.JobEngine;

namespace System.Application.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("data")]
        [ProducesResponseType(typeof(ResponseList<ProcessDto>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] ProcessFilter filter)
        {
            return Json(filter.Filter(_processRepository.GetAllReadData()).Select(x => _mapper.Map<ProcessDto>(x)));
        }

        [HttpPost("{id}/restart")]
        public async Task<IActionResult> Reschedule(Guid id)
        {
            var process = await _processRepository.Get(id);
            if (process is null)
            {
                return NotFound();
            }

            var schema = JsonConvert.DeserializeObject<ProcessSchema>(process.Schema ?? string.Empty, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            if (schema is null)
            {
                return BadRequest();
            }

            RestartSchema(schema);

            await _jobEngine.Execute(schema, await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString(), Role = "-" });

            return Ok();
        }

        private void RestartSchema(ProcessSchema schema)
        {
            schema.Process.Id = Guid.NewGuid();
            schema.Process.Title += " (Restarted)";

            foreach (var job in schema.Jobs)
            {
                var processJob = schema.Process.GetJob(job.Id);
                if (processJob is null)
                    continue;

                job.Id = Guid.NewGuid();
                processJob.Id = job.Id;
            }
        }
    }
}