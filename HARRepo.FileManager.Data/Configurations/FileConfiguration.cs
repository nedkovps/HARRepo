using HARRepo.FileManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Configurations
{
    internal sealed class FileConfiguration : BaseEntityConfiguration<File>
    {
        public override void Configure(EntityTypeBuilder<File> builder)
        {
            base.Configure(builder);

            builder.ToTable("Files");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Path)
                .IsRequired();

            builder.HasOne(x => x.Directory)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.DirectoryId);
        }
    }
}
