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
        protected ILogNotifier Notifier => _controllerService.Notifier;

        public BaseController(IControllerService controllerService)
        {
            _controllerService = controllerService;
        }

        protected UserData? CurrentUser => _controllerService.CurrentUser;

        protected IEnumerable<UserData> Users => _controllerService.Users;

        protected IConnect Connect => _controllerService.Connect;

        protected BadRequestObjectResult BadRequest(int messageId, params object[] args)
            => base.BadRequest(new LogMessage(messageId, args));

        protected BadRequestObjectResult BadRequest(DictionaryItem message, params object[] args)
            => BadRequest(message.Key, args);

        protected BadRequestObjectResult BadRequest(IEnumerable<int> messageIds)
            => base.BadRequest(messageIds.Select(id => new LogMessage(id, Array.Empty<object>())).ToArray());

        protected IActionResult Json<T>(IEnumerable<T> data, int? count = null)
            => Json(new ResponseList<T>
            {
                Data = data.ToList(),
                Total = count ?? data.Count()
            });
    }

    public sealed record LogMessage(int MessageId, object[] Args);
}
