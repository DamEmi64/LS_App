using Microsoft.AspNetCore.Mvc;

namespace Base
{
    /// <summary>
    ///     Base API controller exposing shared user, notification, and list-response helpers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        private readonly IControllerService _controllerService;
        protected INotifier Notifier => _controllerService.Notifier;

        public BaseController(IControllerService controllerService)
        {
            _controllerService = controllerService;
        }

        protected UserData? CurrentUser => _controllerService.CurrentUser;

        protected IEnumerable<UserData> Users => _controllerService.Users;

        protected IConnect Connect => _controllerService.Connect;

        protected IActionResult Json<T>(IEnumerable<T> data, int? count = null)
            => Json(new ResponseList<T>
            {
                Data = data.ToList(),
                Total = count ?? data.Count()
            });
    }
}
