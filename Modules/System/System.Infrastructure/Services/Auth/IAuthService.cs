using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Domain.Entities;
using System.Infrastructure.Services.Auth.Models;

namespace System.Infrastructure.Services.Auth
{
    public interface IAuthService
    {
        Task<IdentityResult> ResetPassword(ResetPasswordModel model);

        Task<IdentityResult> ChangePassword(ChangePasswordModel model, HttpContext context);

        Task<SignInResult> Login(LoginModel model);

        Task Logout();

        Task<IdentityResult> Register(RegisterModel register);

        Task<UserData?> Me(HttpContext context);

        Task<User?> GetUser(HttpContext context);

        Task Update(User data, HttpContext context);

        Task ForgotPassword(string login);
    }
}