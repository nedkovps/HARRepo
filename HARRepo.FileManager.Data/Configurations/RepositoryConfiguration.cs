using HARRepo.FileManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Configurations
{
    internal sealed class RepositoryConfiguration : BaseEntityConfiguration<Repository>
    {
        public override void Configure(EntityTypeBuilder<Repository> builder)
        {
            base.Configure(builder);

            builder.ToTable("Repositories");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Repositories)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Root)
                .WithOne()
                .HasForeignKey<Repository>(x => x.RootId);
        }
    }
}
