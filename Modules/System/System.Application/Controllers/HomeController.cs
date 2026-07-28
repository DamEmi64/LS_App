using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace System.Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMediaProvider _mediaProvider;

        public HomeController(IControllerService controllerService, IMediaProviderFactory mediaProviderFactory) : base(controllerService)
        {
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
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

            if (!string.IsNullOrEmpty(media.Owner))
            {
                return Unauthorized();
            }

            return Json(media);
        }

        [HttpGet("Users")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseList<UserData>), StatusCodes.Status200OK)]
        public async Task<IActionResult?> GetListOfUsers()
        {
            var usersResult = await Connect.GetUsers();

            if (usersResult.IsFailed)
                return BadRequest();

            return Json(usersResult.Value);
        }
    }
}