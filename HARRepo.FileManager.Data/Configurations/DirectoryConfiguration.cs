using HARRepo.FileManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Configurations
{
    internal sealed class DirectoryConfiguration : BaseEntityConfiguration<Directory>
    {
        public override void Configure(EntityTypeBuilder<Directory> builder)
        {
            base.Configure(builder);

            builder.ToTable("Directories");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.SubDirectories)
                .HasForeignKey(x => x.ParentId);

            builder.HasMany(x => x.Files)
                .WithOne(x => x.Directory)
                .HasForeignKey(x => x.DirectoryId);
        }
    }
}
