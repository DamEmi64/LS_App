using Base;
using Communication.Application.Dtos;
using Communication.Application.Filters;
using Communication.Domain.Dictionaries;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Services.SendService;
using Microsoft.AspNetCore.Authorization;
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
            email.Status = EmailStatus.Created;
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

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> WebhookHandle([FromBody] WebhookDto dto)
        {
            if (Guid.TryParse(dto.CustomId, out var customId))
            {
                var email = await _emailRepository.Get(customId);

                if (email is not null)
                {
                    email.Status = ConvertWebhookStatus(dto.Event ?? string.Empty);
                }
            }

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

            var process = await _sendService.SendMail(email.ToSingleItemList(), CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(Base.NotifyTypes.ProcessQueued, process);

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

            var process = await _sendService.SendMail(email.ToSingleItemList(), CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(Base.NotifyTypes.ProcessQueued, process);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailRepository.Remove(id);

            return Ok();
        }

        private int ConvertWebhookStatus(string status)
            => status switch
            {
                "open" => Domain.Dictionaries.EmailStatus.Open,
                "sent" => Domain.Dictionaries.EmailStatus.SentConfirmed,
                "rejected" => Domain.Dictionaries.EmailStatus.Rejected,
                _ => Domain.Dictionaries.EmailStatus.Created
            };
    }
}