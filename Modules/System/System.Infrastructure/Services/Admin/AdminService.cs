using Base.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Security.Claims;

namespace System.Infrastructure.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogRepository _logRepository;
        private readonly IDictionaryRepository _dictionaryRepository;

        public AdminService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogRepository logRepository,
            IDictionaryRepository dictionaryRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logRepository = logRepository;
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<UserInfo?> GetUser(string id)
        {
            var roles = _roleManager.Roles.ToList();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            user.Email = await _userManager.GetEmailAsync(user);

            var role = await _userManager.GetRolesAsync(user);
            return new UserInfo
            {
                User = user,
                Role = roles.FirstOrDefault(r => _userManager.IsInRoleAsync(user, r.Name ?? string.Empty).Result)
            };
        }

        public async Task<IdentityRole?> GetRole(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public IEnumerable<UserInfo> GetUsers()
        {
            var users = _userManager.Users.ToList();

            users.ForEach(u => u.Email = _userManager.GetEmailAsync(u).Result);

            var roles = _roleManager.Roles.ToList();
            return users.Select(u => new UserInfo
            {
                User = u,
                Role = roles.FirstOrDefault(r => _userManager.IsInRoleAsync(u, r.Name ?? string.Empty).Result)
            }).ToList();
        }

        public async Task<IdentityResult> UpdateUser(User user, string role)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return result;

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        public async Task<IdentityResult> DeleteUser(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(User user, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> CreateRole(string roleName, IEnumerable<string> permissions)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed();
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return result;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var permission in permissions)
            {
                await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }

            return result;
        }

        public async Task<IdentityResult> UpdateRole(IdentityRole role, IEnumerable<string> permissions)
        {
            var result = await _roleManager.UpdateAsync(role);
            await _roleManager.UpdateNormalizedRoleNameAsync(role);
            if (!result.Succeeded)
                return result;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var permission in permissions)
            {
                await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }

            return result;
        }

        public async Task<IdentityResult> DeleteRole(IdentityRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public IEnumerable<Log> GetLogs()
        {
            return _logRepository.GetAll();
        }

        public async Task<IList<Claim>> GetRolePermissions(IdentityRole role)
        {
            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<IdentityResult> UpdateRole(string role, IEnumerable<string> permissions)
        {
            var identityRole = _roleManager.Roles.FirstOrDefault(x => x.Name == role);

            if (identityRole is null)
                return IdentityResult.Failed();

            return await UpdateRole(identityRole, permissions);
        }

        public IEnumerable<DictionaryItem> GetDictionaries() => _dictionaryRepository.GetAll();
    }
}