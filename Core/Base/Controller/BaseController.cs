using Microsoft.AspNetCore.Mvc;

namespace Base
{
    public class BaseController : Controller
    {
        private readonly IControllerService _controllerService;
        protected INotifier Notifier => _controllerService.Notifier;

        public BaseController(IControllerService controllerService)
        {
            _controllerService = controllerService;
        }

        protected Task<UserData?> GetCurrentUser()
        {
            return _controllerService.GetUser(HttpContext);
        }

        protected IActionResult Json<T>(IEnumerable<T> data, int? count = null)
            => Json(new ResponseList<T>
            {
                Data = data.ToList(),
                Total = count ?? data.Count()
            });
    }
}