using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Application.Dtos;
using System.Application.Filters;
using System.Data;
using System.Infrastructure.Filters;
using System.Infrastructure.Services.Admin;
using System.Infrastructure.Services.Auth;
using System.Infrastructure.Services.Auth.Models;

namespace System.Application.Controllers
{
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly ILogger<AdminController> _logger;
        private readonly IConnectorResolver _connector;

        public AdminController(IControllerService controllerService,
            IAdminService adminService,
            IAuthService authService,
            ILogger<AdminController> logger,
            IConnectorResolver connector) : base(controllerService)
        {
            _adminService = adminService;
            _authService = authService;
            _logger = logger;
            _connector = connector;
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpGet("[action]")]
        public IActionResult UserList([FromQuery] DataTablesFilterDto request)
        {
            var users = _adminService.GetUsers();

            var query = users;

            // Filtering
            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(r => r.User.UserName?.ToLower().Contains(search) ?? false);
            }

            var filtered = query.Count();

            // Paging
            var data = query
                .Skip(request.Start ?? 0)
                .Take(request.Length ?? 10)
            .ToList();

            return Json(new
            {
                draw = request.Draw,
                recordsTotal = users.Count(),
                recordsFiltered = filtered,
                data = data.Select(x => new
                {
                    Id = x.User.Id,
                    x.User.Email,
                    x.User.UserName,
                    x.User.FirstName,
                    x.User.LastName,
                    Role = x.Role?.NormalizedName ?? "-",
                    LockedOut = x.User.LockoutEnd != null
                })
            });
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser([FromQuery] string id)
        {
            var userInfo = await _adminService.GetUser(id);

            if (userInfo is null)
            {
                return NotFound();
            }

            var user = userInfo.User;
            var role = userInfo.Role;

            return Json(new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.FirstName,
                user.LastName,
                Role = role,
                LockedOut = user.LockoutEnabled
            });
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromForm] RegisterModel register)
        {
            var result = await _authService.Register(register);

            return ToActionResult(result);
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateUser([FromForm] string id, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string email, [FromForm] string? locked, [FromForm] string role)
        {
            try
            {
                var userInfor = await _adminService.GetUser(id);
                var user = userInfor?.User;

                if (user == null)
                    return NotFound("User not found.");

                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.LockoutEnd = locked == "on" ? DateTimeOffset.MaxValue : default(DateTimeOffset?);

                var result = await _adminService.UpdateUser(user, role);

                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure updating user {userId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteUser([FromForm] string id)
        {
            var userInfo = await _adminService.GetUser(id);
            var user = userInfo?.User;

            if (user == null)
                return NotFound("User not found.");

            var successed = await _adminService.DeleteUser(user);

            return successed.Succeeded ? Ok() : BadRequest($"{successed}");
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromForm] string id, [FromForm] string password, [FromForm] string verify)
        {
            try
            {
                if (password != verify)
                    return BadRequest("Passwords entered do not match.");

                var user = await _adminService.GetUser(id);

                if (user is null)
                {
                    return NotFound();
                }

                var result = await _adminService.ResetPassword(user.User, password);

                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed password reset for user {userId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRole([FromForm] RoleDto role)
        {
            try
            {
                var result = await _adminService.CreateRole(role.Name, role.Claims.Select(x => x.Key));

                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure creating role {name}.", role.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpGet("[action]")]
        public async Task<IActionResult> RoleList([FromQuery] DataTablesFilterDto request)
        {
            var roles = _adminService.GetRoles();
            var query = roles;

            // Filtering
            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(r => r.Name?.ToLower().Contains(search) ?? false);
            }

            var filtered = query.Count();

            // Paging
            var data = query
                .Skip(request.Start ?? 0)
                .Take(request.Length ?? 10)
                .Select(r => new { r.Id, r.Name })
            .ToList();

            return Json(new
            {
                draw = request.Draw,
                recordsTotal = roles.Count(),
                recordsFiltered = filtered,
                data = data
            });
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetRole([FromQuery] string id)
        {
            var role = await _adminService.GetRole(id);
            if (role == null)
                return NotFound("Role not found.");

            var claims = await _adminService.GetRolePermissions(role);
            return Json(new
            {
                role.Id,
                role.Name,
                claims = _connector.Permissions.Where(c => claims.Select(x => x.Value).Contains(c.Key)).Select(c => new { c.Key, c.Description })
            });
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateRole([FromForm] RoleDto dto)
        {
            try
            {
                var role = await _adminService.GetRole(dto.Id ?? string.Empty);
                if (role == null)
                    return NotFound("Role not found.");

                role.Name = dto.Name;

                var result = await _adminService.UpdateRole(role, dto.Claims.Select(x => x.Key));

                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure updating role {roleId}.", dto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRole([FromForm] string id)
        {
            try
            {
                var role = await _adminService.GetRole(id);
                if (role == null)
                    return NotFound("Role not found.");

                var result = await _adminService.DeleteRole(role);

                return ToActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure delete role {roleId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [TypeFilter(typeof(AdminPanelFilter))]
        [HttpGet("[action]")]
        public IActionResult LogsData([FromQuery] LogFilter request, string? level, string? method)
        {
            var query = _adminService.GetLogs();

            if (!string.IsNullOrEmpty(level))
                query = query.Where(l => l.Level == level);

            if (!string.IsNullOrEmpty(method))
                query = query.Where(l => l.HttpMethod?.ToLower() == method?.ToLower());

            // Search
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                string term = request.Search.Value.ToLower();
                query = query.Where(l =>
                    l.Message.ToLower().Contains(term) ||
                    (l.User is not null &&
                    l.User.ToLower().Contains(term)) ||
                    (l.HttpUri is not null &&
                    l.HttpUri.ToLower().Contains(term)) ||
                    (l.Exception is not null &&
                    l.Exception.ToLower().Contains(term)));
            }

            int total = query.Count();

            // Sorting
            var sorted = query.OrderByDescending(l => l.TimeStamp);

            // Paging
            var data = sorted
                .Skip(request.Start)
                .Take(request.Length)
                .Select(l => new
                {
                    l.Id,
                    l.Level,
                    l.TimeStamp,
                    l.HttpUri,
                    l.HttpMethod,
                    l.User,
                    l.Message,
                    l.Exception
                })
                .ToList();

            return Json(new
            {
                draw = request.Draw,
                recordsTotal = total,
                recordsFiltered = total,
                data
            });
        }

        private IActionResult ToActionResult(IdentityResult result) => result.Succeeded ? Ok() : BadRequest(result.Errors.Select(x => x.Description));
    }
}