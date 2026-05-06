using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace System.Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConnectorResolver _connector;
        private readonly IMediaProvider _mediaProvider;

        public HomeController(IControllerService controllerService, IConnectorResolver connector, IMediaProvider mediaProvider) : base(controllerService)
        {
            _connector = connector;
            _mediaProvider = mediaProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var modules = _connector.Modules.Select(x => new { x.Name, x.Version });
            var version = _connector.Version;

            return Json(new { modules, version });
        }

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok();

        [HttpGet("image")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult?> GetImage([FromQuery] Guid id)
        {
            var media = await _mediaProvider.Load(id);

            if (media is null || !media.IsImage())
            {
                return Json(new Base.Media());
            }

            return Json(media);
        }
    }
}