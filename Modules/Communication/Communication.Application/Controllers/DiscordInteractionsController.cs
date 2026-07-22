using Base;
using Communication.Domain;
using Communication.Domain.Repositories;
using Communication.Infrastructure;
using CommunicationBase;
using CommunicationBase.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Serilog;
using System.Text;
using System.Text.Json;

namespace DiscordBot.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/discord/interactions")]
    public class DiscordInteractionsController : BaseController
    {
        private readonly string _publicKeyHex;
        private readonly IDiscordRepository _discordRepository;
        private readonly ILogger _logger;

        public DiscordInteractionsController(IControllerService controllerService,
            IOptions<DiscordOptions> options,
            IDiscordRepository discordRepository,
            ILogger logger)
            : base(controllerService)
        {
            _publicKeyHex = options.Value.PublicKey;
            _discordRepository = discordRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string rawBody;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            if (!VerifySignature(rawBody))
            {
                return Unauthorized();
            }

            using var doc = JsonDocument.Parse(rawBody);
            var root = doc.RootElement;
            int type = root.GetProperty("type").GetInt32();

            switch (type)
            {
                case Constants.TYPE_PING:
                    return Ok(new { type = Constants.RESPONSE_PONG });

                case Constants.TYPE_APPLICATION_COMMAND:
                    return await HandleApplicationCommand(root);

                case Constants.TYPE_MESSAGE_COMPONENT:
                    return HandleMessageComponent(root);

                case Constants.TYPE_APPLICATION_COMMAND_AUTOCOMPLETE:
                    return HandleAutocomplete(root);

                case Constants.TYPE_MODAL_SUBMIT:
                    return HandleModalSubmit(root);

                default:
                    _logger.Warning("Invalid discord type");
                    return BadRequest();
            }
        }

        private async Task<IActionResult> HandleApplicationCommand(JsonElement root)
        {
            string commandName = root
                .GetProperty("data")
                .GetProperty("name")
                .GetString() ?? string.Empty;

            var result = await Connect.ExecuteDiscordCmdAsync(commandName, root.ToCommandContext());

            if (result.IsFailed)
            {
                _logger.Error(string.Join(", ", result.Errors.Select(x => x.Message)));
                return BadRequest();
            }

            var response = result.Value;
            // Plain text-only response — normal JSON body, no attachments.
            if (response.Files is null || response.Files.Count == 0)
            {
                return Ok(new
                {
                    type = Constants.RESPONSE_CHANNEL_MESSAGE_WITH_SOURCE,
                    data = new
                    {
                        content = response.Text
                    }
                });
            }
            var i = 0;
            var payload = new
            {
                type = Constants.RESPONSE_CHANNEL_MESSAGE_WITH_SOURCE,
                data = new
                {
                    content = response.Text,
                    attachments = response.Files.Select(x => new
                    {
                        id = i++,
                        filename = x.Filename
                    })
                }
            };

            return BuildMultipartInteractionResponse(payload, response.Files);
        }

        /// <summary>
        /// Builds a multipart/form-data HTTP response for an interaction that includes file attachments.
        /// Discord requires this format instead of plain JSON whenever "attachments" are present:
        /// - part named "payload_json" containing the normal interaction response body
        /// - one part per file named "files[0]", "files[1]", etc.
        /// </summary>
        private IActionResult BuildMultipartInteractionResponse(object payload, IReadOnlyList<DiscordResponse.DiscordResponseFile> files)
        {
            string boundary = $"----DiscordBoundary{Guid.NewGuid():N}";
            using var stream = new MemoryStream();

            void WriteLine(string text)
            {
                var bytes = Encoding.UTF8.GetBytes(text + "\r\n");
                stream.Write(bytes, 0, bytes.Length);
            }

            // payload_json part
            WriteLine($"--{boundary}");
            WriteLine("Content-Disposition: form-data; name=\"payload_json\"");
            WriteLine("Content-Type: application/json");
            WriteLine("");
            WriteLine(JsonSerializer.Serialize(payload));

            // file parts
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                WriteLine($"--{boundary}");
                WriteLine($"Content-Disposition: form-data; name=\"files[{i}]\"; filename=\"{file.Filename}\"");
                WriteLine($"Content-Type: {file.Extension.ToContentType()}");
                WriteLine("");
                stream.Write(file.Content, 0, file.Content.Length);
                WriteLine("");
            }

            WriteLine($"--{boundary}--");

            return File(stream.ToArray(), $"multipart/form-data; boundary={boundary}");
        }

        private IActionResult HandleMessageComponent(JsonElement root)
        {
            string customId = root
                .GetProperty("data")
                .GetProperty("custom_id")
                .GetString() ?? string.Empty;

            return Ok(new
            {
                type = Constants.RESPONSE_UPDATE_MESSAGE,
                data = new
                {
                    content = $"You clicked: {customId}"
                }
            });
        }

        private IActionResult HandleAutocomplete(JsonElement root)
        {
            var commands = _discordRepository.GetAll().Where(x => x.Active);
            return Ok(new
            {
                type = 8, // APPLICATION_COMMAND_AUTOCOMPLETE_RESULT
                data = new
                {
                    choices = commands.Select(x => new
                    {
                        name = x.Cmd,
                        value = x.Cmd
                    })
                }
            });
        }

        private IActionResult HandleModalSubmit(JsonElement root)
        {
            string customId = root
                .GetProperty("data")
                .GetProperty("custom_id")
                .GetString() ?? string.Empty;

            return Ok(new
            {
                type = Constants.RESPONSE_CHANNEL_MESSAGE_WITH_SOURCE,
                data = new
                {
                    content = "Thanks, your submission was received!"
                }
            });
        }

        /// <summary>
        /// Verifies the Ed25519 signature Discord attaches to every interaction request.
        /// Required so Discord (and only Discord) can call this endpoint.
        /// </summary>
        private bool VerifySignature(string rawBody)
        {
            if (!Request.Headers.TryGetValue("X-Signature-Ed25519", out var signatureHeader) ||
                !Request.Headers.TryGetValue("X-Signature-Timestamp", out var timestampHeader))
            {
                return false;
            }

            try
            {
                var publicKeyBytes = Convert.FromHexString(_publicKeyHex);
                var signatureBytes = Convert.FromHexString(signatureHeader.ToString());
                var message = Encoding.UTF8.GetBytes(timestampHeader + rawBody);

                var publicKeyParams = new Ed25519PublicKeyParameters(publicKeyBytes, 0);
                var verifier = new Ed25519Signer();
                verifier.Init(forSigning: false, publicKeyParams);
                verifier.BlockUpdate(message, 0, message.Length);

                return verifier.VerifySignature(signatureBytes);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
