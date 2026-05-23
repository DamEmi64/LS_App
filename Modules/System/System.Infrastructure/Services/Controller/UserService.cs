using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Domain.Entities;
namespace System.Infrastructure.Services.Controller
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<UserData> Users => _userManager.Users.Select(u => new UserData
        {
            Email = u.Email,
            UserId = u.Id,
            Id = 0,
            Login = u.UserName
        });
    }
}
