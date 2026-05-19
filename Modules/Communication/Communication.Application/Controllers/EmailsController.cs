using Base;
using Communication.Application.Filters;
using Communication.Domain.Entities;
using Communication.Infrastructure.Services.SendService;
using Files.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Application.Controllers
{
    [AuthPermission("communication")]
    public class EmailsController : BaseController
    {
        private readonly IEmailRepository _emailRepository;
        private readonly ISendService _sendService;

        public EmailsController(
            IControllerService controllerService,
            IEmailRepository emailRepository,
            ISendService sendService)
            : base(controllerService)
        {
            _emailRepository = emailRepository;
            _sendService = sendService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _emailRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Email>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] EmailFilter filter)
        {
            return Json(filter.Filter(_emailRepository.GetAll(), out var count), count);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] Email email)
        {
            await _emailRepository.Add(email);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Email emailData)
        {
            var email = await _emailRepository.Get(id);

            ArgumentNullException.ThrowIfNull(email, nameof(email));

            email.Subject = emailData.Subject;
            email.SentDate = emailData.SentDate;
            email.Body = emailData.Body;
            email.UpdDate = DateTimeOffset.Now;
            email.Recipient = emailData.Recipient;

            await _emailRepository.Update(email);

            return Ok();
        }

        [HttpPut("{id}/send")]
        public async Task<IActionResult> Send([FromRoute] Guid id)
        {
            var email = await _emailRepository.Get(id);

            if (email is null)
            {
                return NotFound();
            }

            var process = await _sendService.SendMail(email.ToSingleItemList(), await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(NotifyTypes.ProcessQueued, process);

            return Ok();
        }

        [HttpPut("{id}/sendExternal")]
        public async Task<IActionResult> SendExternal([FromQuery] string sender, [FromQuery] string recipient, [FromQuery] string subject, [FromBody] string content)
        {
            var email = new Email()
            {
                Body = content,
                Sender = sender,
                Recipient = recipient,
                Subject = subject,
                InsDate = DateTime.Now,
                UpdDate = DateTime.Now
            };

            await _emailRepository.Add(email);

            var process = await _sendService.SendMail(email.ToSingleItemList(), await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(NotifyTypes.ProcessQueued, process);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailRepository.Remove(id);

            return Ok();
        }
    }
}