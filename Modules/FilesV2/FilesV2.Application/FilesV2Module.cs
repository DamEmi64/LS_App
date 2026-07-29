using Base;
using FilesV2.Infrastructure;
using FilesV2.Infrastructure.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilesV2.Application
{
    public class FilesV2Module : IModule
    {
        public IEnumerable<Operation> Operations => [];

        public string Name => "Files V2";

        public string Version => "v0.0.1";

        public IEnumerable<PermissionInfo> Permissions => [PermissionInfo.Create("filesV2","manages files"),
                                                         PermissionInfo.Create("directoriesV2","manage directories")];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddRepos();
            services.AddDatabase<FilesV2Context>(AppConfiguration.DefaultConnectionString);
            return services;
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}
