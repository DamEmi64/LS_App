using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Domain.Dictionaries;
using System.Domain.Entities;
using System.Infrastructure.Services.Auth;
using System.Infrastructure.Services.Auth.Models;

namespace System.Application.Controllers
{
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IControllerService controllerService, IAuthService authService) : base(controllerService)
        {
            _authService = authService;
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserData), StatusCodes.Status200OK)]
        public async Task<UserData?> GetInfo()
        {
            return await _authService.Me(HttpContext);
        }

        [HttpGet("data")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<User?> GetUser()
        {
            return await _authService.GetUser(HttpContext);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);

            if (result.Succeeded)
            {
                await _authService.Logout();
                await _authService.Login(new LoginModel { Login = model.Login, Password = model.Password });

                return Ok(result);
            }

            return BadRequest(GetNotifications(result.Errors).Select(x => x.ToString()).ToList());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.Login(model);

            return result switch
            {
                _ when result.Succeeded => Ok(),
                _ when result.IsLockedOut => BadRequest(SystemNotifyTypes.LockedOut),
                _ when result.IsNotAllowed => BadRequest(SystemNotifyTypes.LoginFailed),
                _ when result.RequiresTwoFactor => BadRequest(SystemNotifyTypes.TwoFactorFailed),
                _ => BadRequest(SystemNotifyTypes.LoginFailed)
            };
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ResetPasswordModel model)
        {
            var result = await _authService.ChangePassword(model, HttpContext);

            return result switch
            {
                _ when result.Succeeded => Ok(),
                _ => BadRequest(string.Join("\n", result.Errors.Select(e => e.Description)))
            };
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] User data)
        {
            await _authService.Update(data, HttpContext);

            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            await _authService.Logout();
            return Ok();
        }

        private IEnumerable<int> GetNotifications(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                yield return error.Code switch
                {
                    "PasswordTooShort" => SystemNotifyTypes.PasswordTooShort,
                    "PasswordRequiresNonAlphanumeric" => SystemNotifyTypes.PasswordRequiresNonAlphanumeric,
                    "PasswordRequiresDigit" => SystemNotifyTypes.PasswordRequiresDigit,
                    "PasswordRequiresLower" => SystemNotifyTypes.PasswordRequiresLower,
                    "PasswordRequiresUpper" => SystemNotifyTypes.PasswordRequiresUpper,
                    _ => NotifyTypes.Log
                };
            }
        }
    }
}