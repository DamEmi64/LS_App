using Base;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Infrastructure.Services.Auth.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace System.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private const string TokenProvider = "Jwt";
        private const string RefreshTokenName = "RefreshToken";
        private const string RefreshTokenExpiresAtName = "RefreshTokenExpiresAt";

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserStore<User> _userStore;
        private readonly IConnect _connect;
        private readonly IConfiguration _configuration;

        public AuthService(SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthService> logger,
            IUserStore<User> userStore,
            RoleManager<IdentityRole> roleManager,
            IConnect connect,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userStore = userStore;
            _roleManager = roleManager;
            _connect = connect;
            _configuration = configuration;
        }

        public async Task<Result<Token>> Login(LoginModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var user = await _userManager.FindByNameAsync(model.Login);

            if (user is null)
            {
                return Result.Fail<Token>("LoginFailed");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.IsLockedOut)
            {
                return Result.Fail<Token>("LockedOut");
            }

            if (result.IsNotAllowed)
            {
                return Result.Fail<Token>("LoginFailed");
            }

            if (result.RequiresTwoFactor)
            {
                return Result.Fail<Token>("TwoFactorFailed");
            }

            return result.Succeeded
                ? Result.Ok(await CreateToken(user))
                : Result.Fail<Token>("LoginFailed");
        }

        public async Task<Result<Token>> RefreshToken(RefreshTokenModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
            {
                return Result.Fail<Token>("InvalidRefreshToken");
            }

            var savedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, TokenProvider, RefreshTokenName);
            var expiresAtValue = await _userManager.GetAuthenticationTokenAsync(user, TokenProvider, RefreshTokenExpiresAtName);

            if (string.IsNullOrWhiteSpace(savedRefreshToken) ||
                savedRefreshToken != model.RefreshToken ||
                !DateTimeOffset.TryParse(expiresAtValue, out var expiresAt) ||
                expiresAt <= DateTimeOffset.UtcNow)
            {
                return Result.Fail<Token>("InvalidRefreshToken");
            }

            return Result.Ok(await CreateToken(user));
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

                if (!await _roleManager.RoleExistsAsync(register.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(register.Role));
                }

                await _userManager.AddToRoleAsync(user, register.Role);
            }

            return result;
        }

        public async Task Logout(HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user is null)
            {
                return;
            }

            await _userManager.RemoveAuthenticationTokenAsync(user, TokenProvider, RefreshTokenName);
            await _userManager.RemoveAuthenticationTokenAsync(user, TokenProvider, RefreshTokenExpiresAtName);
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

            await _connect.Send(new SendEmail(user.Email, "Reset password", html, Register: false));
        }

        private async Task<Token> CreateToken(User user)
        {
            var jwtSection = _configuration.GetSection("config").GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(jwtSection.GetValue("ExpiresMinutes", 60));
            var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(jwtSection.GetValue("RefreshExpiresDays", 14));
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is null)
                {
                    continue;
                }

                claims.AddRange(await _roleManager.GetClaimsAsync(role));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials);

            await _userManager.SetAuthenticationTokenAsync(user, TokenProvider, RefreshTokenName, refreshToken);
            await _userManager.SetAuthenticationTokenAsync(user, TokenProvider, RefreshTokenExpiresAtName, refreshTokenExpiresAt.ToString("O"));

            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserId = user.Id,
                ExpiresAt = expiresAt,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }
    }
}
