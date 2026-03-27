using Microsoft.AspNetCore.Identity;
using System.Domain.Entities;

namespace System.Infrastructure.Services.Admin
{
    public class UserInfo
    {
        public required User User { get; set; }
        public IdentityRole? Role { get; set; }
    }
}