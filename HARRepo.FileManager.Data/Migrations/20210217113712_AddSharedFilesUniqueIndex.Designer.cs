﻿// <auto-generated />
using System;
using HARRepo.FileManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HARRepo.FileManager.Data.Migrations
{
    [DbContext(typeof(HARRepoFileManagerContext))]
    [Migration("20210217113712_AddSharedFilesUniqueIndex")]
    partial class AddSharedFilesUniqueIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("DirectoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.Repository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("LastActivityOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RootId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RootId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.SharedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<int>("SharedWithId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("SharedWithId");

                    b.HasIndex("FileId", "OwnerId", "SharedWithId")
                        .IsUnique();

                    b.ToTable("SharedFiles");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.Directory", b =>
                {
                    b.HasOne("HARRepo.FileManager.Data.Entities.Directory", "Parent")
                        .WithMany("SubDirectories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.File", b =>
                {
                    b.HasOne("HARRepo.FileManager.Data.Entities.Directory", "Directory")
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.Repository", b =>
                {
                    b.HasOne("HARRepo.FileManager.Data.Entities.Directory", "Root")
                        .WithOne()
                        .HasForeignKey("HARRepo.FileManager.Data.Entities.Repository", "RootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HARRepo.FileManager.Data.Entities.User", "User")
                        .WithMany("Repositories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Root");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.SharedFile", b =>
                {
                    b.HasOne("HARRepo.FileManager.Data.Entities.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HARRepo.FileManager.Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HARRepo.FileManager.Data.Entities.User", "SharedWith")
                        .WithMany("SharedFilesWithUser")
                        .HasForeignKey("SharedWithId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Owner");

                    b.Navigation("SharedWith");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.Directory", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("SubDirectories");
                });

            modelBuilder.Entity("HARRepo.FileManager.Data.Entities.User", b =>
                {
                    b.Navigation("Repositories");

                    b.Navigation("SharedFilesWithUser");
                });
#pragma warning restore 612, 618
        }
    }
}
