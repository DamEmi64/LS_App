using Base;
using FluentResults;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Infrastructure.Services.Admin;
using System.Text;

namespace System.Infrastructure.Connect.ProvideBasicRoles
{
    public class ProvideBasicRoles : ConnectInstance<SharedEvents.Auth.ProvideBasicRoles>
    {
        private readonly IAdminService _adminService;

        public ProvideBasicRoles(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public override async Task HandleAsync(SharedEvents.Auth.ProvideBasicRoles request)
        {
            var roles = _adminService.GetRoles();
            if (!roles.Select(x => x.Name).Contains("admin"))
            {
                await _adminService.CreateRole("admin", request.Permissions.Select(x => x.Key));
            }
            else
            {
                await _adminService.UpdateRole("admin", request.Permissions.Select(x => x.Key));
            }

            if (!roles.Select(x => x.Name).Contains("user"))
            {
                await _adminService.CreateRole("user", request.Permissions.Where(x => x.IsBasic).Select(x => x.Key));
            }
            else
            {
                await _adminService.UpdateRole("user", request.Permissions.Where(x => x.IsBasic).Select(x => x.Key));
            }
        }
    }
}