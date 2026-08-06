using Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;

namespace System.Infrastructure.Connect.GetUsers
{
    public class GetUsers : Base.EventHandler<Base.GetUsers, List<UserData>>
    {
        private readonly UserManager<User> _userManager;

        public GetUsers(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public override Task<List<UserData>> HandleAsync(Base.GetUsers request, CancellationToken cancellationToken)
        {
            return _userManager.Users.Select(x => new UserData
            {
                UserId = x.Id,
                Email = x.Email,
                Login = x.UserName,
            }).ToListAsync();
        }
    }
}
