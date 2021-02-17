using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Implementations;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HARRepo.FileManager.Logic.Tests
{
    public class DirectoryManagerTests : TestsBase
    {
        [Fact]
        public async Task ShouldCreateDirectory()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, null);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");

            //Act
            var dirName = "Test Directory";
            var directory = await directoryMnager.CreateDirectoryAsync(dirName, repoRoot.Id);
            var parent = await context.Set<Directory>()
                .Include(x => x.SubDirectories)
                .SingleOrDefaultAsync(x => x.Id == repoRoot.Id);

            //Assert
            Assert.NotNull(directory);
            Assert.True(directory.Id > 0);
            Assert.Equal(dirName, directory.Name);
            Assert.Single(parent.SubDirectories);
            Assert.Equal(parent.SubDirectories.First().Id, directory.Id);
        }

        [Fact]
        public async Task ShouldDeleteDirectoryAndAllChildren()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");
            var directory = await directoryMnager.CreateDirectoryAsync("Test Directory", repoRoot.Id);
            var subDirectory = await directoryMnager.CreateDirectoryAsync("Test Sub Directory", directory.Id);
            var file = await fileManager.UploadFileAsync(directory.Id, "FileName.har", "{}");
            var subFile = await fileManager.UploadFileAsync(subDirectory.Id, "SubFileName.har", "{}");

            //Act
            await directoryMnager.DeleteDirectoryAsync(directory.Id);
            var directoryFromDB = await context.Set<Directory>()
                .FindAsync(directory.Id);
            var subDirectoryFromDB = await context.Set<Directory>()
                .FindAsync(subDirectory.Id);
            var fileFromDB = await context.Set<File>()
                .FindAsync(file.Id);
            var subFileFromDB = await context.Set<File>()
                .FindAsync(subFile.Id);

            //Assert
            Assert.Null(directoryFromDB);
            Assert.Null(subDirectoryFromDB);
            Assert.Null(fileFromDB);
            Assert.Null(subFileFromDB);
        }
    }
}
