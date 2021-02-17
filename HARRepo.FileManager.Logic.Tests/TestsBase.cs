using AutoMapper;
using HARRepo.FileManager.Data;
using HARRepo.FileManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Tests
{
    public class TestsBase
    {
        public IMapper Mapper = GetMapper();
        public IConfigurationRoot Configuration = GetIConfigurationRoot();

        public HARRepoFileManagerContext GetDbContext(bool useSqlLite)
        {
            var builder = new DbContextOptionsBuilder<HARRepoFileManagerContext>()
                .EnableSensitiveDataLogging();
            var dbGuid = Guid.NewGuid().ToString();

            if (useSqlLite)
            {
                //Use SqlLite DB
                builder.UseSqlite("DataSource=:memory:");
            }
            else
            {
                //Use In-Memory DB
                builder.UseInMemoryDatabase(dbGuid);
            }

            var context = new HARRepoFileManagerContext(builder.Options);

            if (useSqlLite)
            {
                context.Database.OpenConnection();
            }

            context.Database.EnsureCreated();

            return context;
        }

        public async Task<User> SeedUserAsync(DbContext context)
        {
            var user = await context.AddAsync(new User() { 
                Email = $"{GetRandomTestName()}@test.com",
                Name = GetRandomTestName()
            });
            await context.SaveChangesAsync();
            return user.Entity;
        }

        public static string GetRandomTestName() => $"test-{Guid.NewGuid():N}";

        public static IMapper GetMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
                cfg.AddMaps(new[]
                {
                    "HARRepo.FileManager.Logic"
                }));

            return mockMapper.CreateMapper();
        }

        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", optional: true)
                .Build();
        }
    }
}
