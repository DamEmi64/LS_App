using Base;
using Communication.Application.Filters;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Application.Controllers
{
    public class CommunicationHistoryController : BaseController
    {
        private readonly ICommunicationHistoryRepository _communicationHistoryRepository;

        public CommunicationHistoryController(IControllerService controllerService, ICommunicationHistoryRepository communicationHistoryRepository) : base(controllerService)
        {
            _communicationHistoryRepository = communicationHistoryRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommunicationHistory), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _communicationHistoryRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<CommunicationHistory>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] HistoryFilter filter)
        {
            return Json(filter.Filter(_communicationHistoryRepository.GetAll(), out var count), count);
        }
    }
}
