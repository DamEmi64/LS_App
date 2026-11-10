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

        [HttpGet("user")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<User?> GetUser()
        {
            return await _authService.GetUser(HttpContext);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel dto)
        {
            var result = await _authService.Register(dto);

            if (result.Succeeded)
            {
                var login = await _authService.Login(new LoginModel { Login = dto.Login, Password = dto.Password });

                return login.IsSuccess ? Ok(login.Value) : BadRequest(SystemLogs.LoginFailed);
            }

            return BadRequest(GetNotifications(result.Errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel dto)
        {
            var result = await _authService.Login(dto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Errors.FirstOrDefault()?.Message switch
            {
                "LockedOut" => BadRequest(SystemLogs.LockedOut),
                "TwoFactorFailed" => BadRequest(SystemLogs.TwoFactorFailed),
                _ => BadRequest(SystemLogs.LoginFailed)
            };
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel dto)
        {
            var result = await _authService.RefreshToken(dto);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(HttpContext);
            return Ok();
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel dto)
        {
            var result = await _authService.ChangePassword(dto, HttpContext);

            return result switch
            {
                _ when result.Succeeded => Ok(),
                _ => BadRequest(SystemLogs.PasswordChangeFailed, string.Join("\n", result.Errors.Select(e => e.Description)))
            };
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel dto)
        {
            var result = await _authService.ResetPassword(dto);

            return result switch
            {
                _ when result.Succeeded => Ok(),
                _ => BadRequest(SystemLogs.PasswordResetFailed, string.Join("\n", result.Errors.Select(e => e.Description)))
            };
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            await _authService.Update(user, HttpContext);

            return Ok();
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string username)
        {
            await _authService.ForgotPassword(username);
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            await _authService.Logout(HttpContext);
            return Ok();
        }

        private IEnumerable<int> GetNotifications(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                yield return error.Code switch
                {
                    "PasswordTooShort" => SystemLogs.PasswordTooShort,
                    "PasswordRequiresNonAlphanumeric" => SystemLogs.PasswordRequiresNonAlphanumeric,
                    "PasswordRequiresDigit" => SystemLogs.PasswordRequiresDigit,
                    "PasswordRequiresLower" => SystemLogs.PasswordRequiresLower,
                    "PasswordRequiresUpper" => SystemLogs.PasswordRequiresUpper,
                    _ => Logs.Log
                };
            }
        }
    }
}

