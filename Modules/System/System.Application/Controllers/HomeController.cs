using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace System.Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMediaProvider _mediaProvider;

        public HomeController(IControllerService controllerService, IMediaProvider mediaProvider) : base(controllerService)
        {
            _mediaProvider = mediaProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var modules = AppConfiguration.Modules.Select(x => new { x.Key, x.Value });
            var version = AppConfiguration.Version;

            return Json(new { modules, version });
        }

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok();

        [HttpGet("media")]
        [Authorize]
        [ProducesResponseType(typeof(Base.Media), StatusCodes.Status200OK)]
        public async Task<IActionResult?> GetMedia([FromQuery] Guid id)
        {
            var media = await _mediaProvider.Load(id);

            if (media is null)
            {
                return Json(new Base.Media());
            }

            return Json(media);
        }
    }
}