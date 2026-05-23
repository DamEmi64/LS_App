using Microsoft.AspNetCore.Mvc;

namespace Base
{
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

        protected IActionResult Json<T>(IEnumerable<T> data, int? count = null)
            => Json(new ResponseList<T>
            {
                Data = data.ToList(),
                Total = count ?? data.Count()
            });
    }
}