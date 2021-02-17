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
    public class FileManagerTests : TestsBase
    {
        [Fact]
        public async Task ShouldUploadFile()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");

            //Act
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");
            var fileFromDB = await context.Set<File>()
                .FindAsync(file.Id);

            //Assert
            Assert.NotNull(fileFromDB);
            Assert.Equal(filePath, fileFromDB.Path);
            Assert.Equal(fileName, fileFromDB.Name);
        }

        [Fact]
        public async Task ShouldUpdateFileLocation()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");
            var directory = await directoryMnager.CreateDirectoryAsync("Test Directory", repoRoot.Id);
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");

            //Act
            await fileManager.UpdateFileLocationAsync(file.Id, directory.Id);
            var fileFromDB = await context.Set<File>()
                .FindAsync(file.Id);

            Assert.NotNull(fileFromDB);
            Assert.Equal(directory.Id, fileFromDB.DirectoryId);
        }

        [Fact]
        public async Task ShouldDeleteFile()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");

            //Act
            await fileManager.DeleteFileAsync(file.Id);
            var fileFromDB = await context.Set<File>()
                .FindAsync(file.Id);

            //Assert
            Assert.Null(fileFromDB);
        }

        [Fact]
        public async Task ShouldDeleteFiles()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");
            var secondFileName = "FileName.har";
            var secondFile = await fileManager.UploadFileAsync(repoRoot.Id, secondFileName, "{}");

            //Act
            var files = await context.Set<File>()
                .Where(x => x.Id == file.Id || x.Id == secondFile.Id)
                .ToListAsync();
            await fileManager.DeleteFilesAsync(files);
            var fileFromDB = await context.Set<File>()
                .FindAsync(file.Id);
            var secondFileFromDB = await context.Set<File>()
                .FindAsync(secondFile.Id);

            //Assert
            Assert.Null(fileFromDB);
            Assert.Null(secondFileFromDB);
        }

        [Fact]
        public async Task ShouldShareFile()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var owner = await SeedUserAsync(context);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(owner)));
            var repoRoot = await repoManager.CreateRepositoryAsync(owner.Id, "Test Repo");
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");

            //Act
            var comment = "Some comment";
            await fileManager.ShareFileAsync(file.Id, owner.Id, user.Id, comment);
            var sharedFileFromDB = await context.Set<SharedFile>()
                .FirstOrDefaultAsync(x => x.FileId == file.Id);

            //Assert
            Assert.NotNull(sharedFileFromDB);
            Assert.Equal(owner.Id, sharedFileFromDB.OwnerId);
            Assert.Equal(user.Id, sharedFileFromDB.SharedWithId);
            Assert.Equal(comment, sharedFileFromDB.Comment);
        }

        [Fact]
        public async Task ShouldGetSharedFiles()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var owner = await SeedUserAsync(context);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(owner)));
            var repoRoot = await repoManager.CreateRepositoryAsync(owner.Id, "Test Repo");
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");
            var comment = "Some comment";
            await fileManager.ShareFileAsync(file.Id, owner.Id, user.Id, comment);

            //Act
            var sharedFileFromDB = await context.Set<SharedFile>()
                .FirstOrDefaultAsync(x => x.FileId == file.Id);
            var sharedFiles = await fileManager.GetUserSharedFilesAsync(owner.Id);
            var sharedFilesWithUser = await fileManager.GetFilesSharedWithUserAsync(user.Id);
            var sharedFilesWithUserCount = await fileManager.GetSharedWithUserFilesCountAsync(user.Id);

            //Assert
            Assert.Single(sharedFiles);
            Assert.Single(sharedFilesWithUser);
            Assert.Equal(1, sharedFilesWithUserCount);
            Assert.Equal(sharedFileFromDB.Id, sharedFiles.First().Id);
            Assert.Equal(sharedFileFromDB.Id, sharedFilesWithUser.First().Id);
        }

        [Fact]
        public async Task ShouldUnshareFileAsync()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var mockFileStorage = new Mock<IFileStorage>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var fileManager = new Implementations.FileManager(context, Mapper, mockFileStorage.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, fileManager);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var filePath = Guid.NewGuid().ToString();
            mockFileStorage.Setup(x => x.UploadAsync(It.IsAny<string>())).Returns(Task.FromResult(filePath));
            mockFileStorage.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var owner = await SeedUserAsync(context);
            var user = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(owner)));
            var repoRoot = await repoManager.CreateRepositoryAsync(owner.Id, "Test Repo");
            var fileName = "FileName.har";
            var file = await fileManager.UploadFileAsync(repoRoot.Id, fileName, "{}");
            var comment = "Some comment";
            await fileManager.ShareFileAsync(file.Id, owner.Id, user.Id, comment);

            //Act
            var sharedFileFromDB = await context.Set<SharedFile>()
                .FirstOrDefaultAsync(x => x.FileId == file.Id);
            await fileManager.UnshareFileAsync(sharedFileFromDB.Id);
            sharedFileFromDB = await context.Set<SharedFile>()
                .FirstOrDefaultAsync(x => x.FileId == file.Id);
            var sharedFilesWithUser = await context.Set<SharedFile>()
                .Where(x => x.SharedWithId == user.Id)
                .ToListAsync();
            var sharedFilesByUser = await context.Set<SharedFile>()
                .Where(x => x.OwnerId == owner.Id)
                .ToListAsync();

            //Assert
            Assert.Null(sharedFileFromDB);
            Assert.Empty(sharedFilesWithUser);
            Assert.Empty(sharedFilesByUser);
        }
    }
}
