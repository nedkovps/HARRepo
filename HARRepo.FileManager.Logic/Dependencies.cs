using HARRepo.FileManager.Data;
using HARRepo.FileManager.Logic.Implementations;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic
{
    public static class Dependencies
    {
        public static void RegisterBusinessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HARRepoFileManagerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HARRepoFileManagerContext")));
            services.AddScoped<DbContext, HARRepoFileManagerContext>();

            services.AddSingleton<IFileStorageLogic, AzureBlobFileStorageLogic>();
        }
    }
}
