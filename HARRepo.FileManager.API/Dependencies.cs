using HARRepo.FileManager.API.Logic;
using HARRepo.FileManager.Logic;
using HARRepo.FileManager.Logic.Implementations;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API
{
    public static class Dependencies
    {
        public static void RegisterApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterBusinessDependencies(configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserResolver, UserResolver>();

            services.AddScoped<IAuthorizationManager, AuthorizationManager>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IDirectoryManager, DirectoryManager>();
            services.AddScoped<IFileManager, FileManager.Logic.Implementations.FileManager>();
        }
    }
}
