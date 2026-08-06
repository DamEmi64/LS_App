using Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;

namespace System.Infrastructure.Connect.GetUserById
{
    public class GetUserByLogin : Base.EventHandler<Base.GetUserByLogin, UserData?>
    {
        private readonly UserManager<User> _userManager;

        public GetUserByLogin(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public override Task<UserData?> HandleAsync(Base.GetUserByLogin request, CancellationToken cancellationToken)
        {
            return _userManager.Users
                    .Where(x => x.UserName == request.login)
                    .Select(x => new UserData
                    {
                        UserId = x.Id,
                        Email = x.Email,
                        Login = x.UserName,
                    }).FirstOrDefaultAsync();
        }
    }
}
