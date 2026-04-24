using Base;
using Microsoft.AspNetCore.Identity;
using System.Domain.Entities;
using System.Security.Claims;

namespace System.Infrastructure.Services.Admin
{
    public interface IAdminService
    {
        IEnumerable<IdentityRole> GetRoles();

        IEnumerable<UserInfo> GetUsers();

        Task<IdentityResult> UpdateUser(User user, string role);

        Task<IdentityResult> DeleteUser(User user);

        Task<IdentityResult> ResetPassword(User user, string newPassword);

        Task<UserInfo?> GetUser(string id);

        Task<IdentityResult> CreateRole(string roleName, IEnumerable<string> permissions);

        Task<IdentityRole?> GetRole(string id);

        Task<IList<Claim>> GetRolePermissions(IdentityRole role);

        Task<IdentityResult> UpdateRole(string role, IEnumerable<string> permissions);
        Task<IdentityResult> UpdateRole(IdentityRole role, IEnumerable<string> permissions);

        Task<IdentityResult> DeleteRole(IdentityRole role);

        IEnumerable<Log> GetLogs();

        IEnumerable<DictionaryItem> GetDictionaries();
    }
}