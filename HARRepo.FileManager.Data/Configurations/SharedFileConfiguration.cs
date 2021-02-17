using HARRepo.FileManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Configurations
{
    internal sealed class SharedFileConfiguration : BaseEntityConfiguration<SharedFile>
    {
        public override void Configure(EntityTypeBuilder<SharedFile> builder)
        {
            base.Configure(builder);

            builder.ToTable("SharedFiles");

            builder.HasIndex(x => new { x.FileId, x.OwnerId, x.SharedWithId })
                .IsUnique();

            builder.HasOne(x => x.File)
                .WithMany()
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SharedWith)
                .WithMany(x => x.SharedFilesWithUser)
                .HasForeignKey(x => x.SharedWithId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
