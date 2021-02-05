using HARRepo.FileManager.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data
{
    public class HARRepoFileManagerContext : DbContext
    {
        public HARRepoFileManagerContext(DbContextOptions<HARRepoFileManagerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //apply all entity configurations (that implement BaseEntityConfiguration)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntityConfiguration<>).Assembly);
        }
    }
}
