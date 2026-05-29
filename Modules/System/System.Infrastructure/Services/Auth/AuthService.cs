using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Domain.Entities;
using System.Infrastructure.Services.Auth.Models;

namespace System.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserStore<User> _userStore;
        private readonly IConnect _connect;

        public AuthService(SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthService> logger,
            IUserStore<User> userStore,
            RoleManager<IdentityRole> roleManager,
            IConnect connect)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userStore = userStore;
            _roleManager = roleManager;
            _connect = connect;
        }

        public async Task<SignInResult> Login(LoginModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            _ = await _userManager.FindByNameAsync(model.Login);

            return await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> Register(RegisterModel register)
        {
            var user = new User();
            user.FirstName = register.FirstName;
            user.LastName = register.LastName;
            await _userStore.SetUserNameAsync(user, register.Login, CancellationToken.None);
            await _userManager.SetEmailAsync(user, register.Email);
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result2 = await _userManager.ConfirmEmailAsync(user, code);

                await _signInManager.SignInAsync(user, isPersistent: false);

                if (!await _roleManager.RoleExistsAsync(register.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(register.Role));
                }

                await _userManager.AddToRoleAsync(user, register.Role);
            }

            return result;
        }

        public Task Logout()
        {
            return _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "User not found" });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            return result;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordModel model, HttpContext context)
        {
            var user = await _userManager.FindByIdAsync(model.UserId ?? string.Empty);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "User not found" });
            }

            return await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        }

        public async Task<UserData?> Me(HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user is null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = _roleManager.Roles.First(x => x.Name == (roles.FirstOrDefault() ?? "user"));
            var claims = await _roleManager.GetClaimsAsync(role);

            return new UserData
            {
                Email = user.Email,
                UserId = user.Id,
                Login = user.UserName,
                Id = 0,
                Role = role.Name ?? string.Empty,
                Permissions = claims.Select(x => x.Value).ToArray()
            };
        }

        public async Task<User?> GetUser(HttpContext context)
        {
            return await _userManager.GetUserAsync(context.User);
        }

        public async Task Update(User data, HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user is not null)
            {
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.PhoneNumber = data.PhoneNumber;

                if (user.Email != data.Email && data.Email is not null)
                {
                    var token = await _userManager.GenerateChangeEmailTokenAsync(user, data.Email);
                    await _userManager.ChangeEmailAsync(user, data.Email, token);
                }

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
            }
        }

        public async Task ForgotPassword(string login)
        {
            var user = await _userManager.FindByNameAsync(login);

            if (user is null || !(user.EmailConfirmed) || user.Email is null)
            {
                return;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var html = $"To reset your password use this code: {code}";

            await _connect.Send(new SendEmail(user.Email, "Reset password", html));
        }
    }
}