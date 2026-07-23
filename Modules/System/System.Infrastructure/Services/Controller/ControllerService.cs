using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Domain.Entities;

namespace System.Infrastructure.Services.Controller
{
    public class ControllerService : IControllerService
    {
        private readonly INotifier _notifier;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public ControllerService(UserManager<User> userManager, INotifier notifier, IHttpContextAccessor contextAccessor, IConnect connect)
        {
            _userManager = userManager;
            _notifier = notifier;
            _contextAccessor = contextAccessor;
            Connect = connect;
        }

        public INotifier Notifier => _notifier;

        public UserData? CurrentUser => _contextAccessor?.HttpContext != null ? GetUser(_contextAccessor.HttpContext).Result : null;

        public IEnumerable<UserData> Users => _userManager.Users.Select(u => new UserData
        {
            Email = u.Email,
            UserId = u.Id,
            Id = 0,
            Login = u.UserName
        });

        public IConnect Connect { get; }

        public async Task<UserData?> GetUser(HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user is null)
                return null;
            var role = await _userManager.GetRolesAsync(user);
            return new UserData
            {
                Email = user.Email,
                UserId = user.Id,
                Id = 0,
                Login = user.UserName,
                Role = role.FirstOrDefault() ?? "user",
                Permissions = context.User.Claims.Select(p => p.Value).ToArray()
            };
        }
    }
}