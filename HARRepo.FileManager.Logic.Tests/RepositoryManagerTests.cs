using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Exceptions;
using HARRepo.FileManager.Logic.Implementations;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HARRepo.FileManager.Logic.Tests
{
    public class RepositoryManagerTests : TestsBase
    {
        [Fact]
        public async Task ShouldAddRepository()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var directoryMnager = new DirectoryManager(context, Mapper, null);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, null);
            var user = await SeedUserAsync(context);

            //Act
            var repoRoot = await repoManager.CreateRepositoryAsync(user.Id, "Test Repo");
            var repo = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);

            //Assert
            Assert.True(repo.Id > 0);
            Assert.Equal("Test Repo", repo.Name);
            Assert.Equal(user.Id, repo.UserId);
            Assert.Equal("Root", repoRoot.Name);
        }

        [Fact]
        public async Task ShouldGetUserRepositories()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var directoryMnager = new DirectoryManager(context, Mapper, null);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, null);
            var user = await SeedUserAsync(context);
            var repoNames = new List<string>() { "Test Repo 1", "Test Repo 2", "Test Repo 3" };
            await repoManager.CreateRepositoryAsync(user.Id, repoNames[0]);
            await repoManager.CreateRepositoryAsync(user.Id, repoNames[1]);
            await repoManager.CreateRepositoryAsync(user.Id, repoNames[2]);

            //Act
            var userRepos = await repoManager.GetUserRepositoriesAsync(user.Id);

            //Assert
            Assert.Equal(3, userRepos.Count);
            Assert.Contains(userRepos[0].Name, repoNames);
            Assert.Contains(userRepos[1].Name, repoNames);
            Assert.Contains(userRepos[2].Name, repoNames);
        }

        [Fact]
        public async Task ShouldGetRepositoryById()
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
            var repoFromDB = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);
            var repo = await repoManager.GetRepositoryAsync(repoFromDB.Id);

            //Assert
            Assert.NotNull(repo);
            Assert.Equal(repoFromDB.Name, repo.Name);
        }

        [Fact]
        public async Task ShouldNotGetOthersRepositoryById()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, null);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var user = await SeedUserAsync(context);
            var owner = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(owner.Id, "Test Repo");

            //Act
            var repoFromDB = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);

            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(() => repoManager.GetRepositoryAsync(repoFromDB.Id));
        }

        [Fact]
        public async Task ShouldDeleteRepository()
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
            var repoFromDB = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);
            await repoManager.DeleteRepositoryAsync(repoFromDB.Id);
            repoFromDB = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);

            Assert.Null(repoFromDB);
        }

        [Fact]
        public async Task ShouldNotDeleteOthersRepository()
        {
            //Arrange
            await using var context = GetDbContext(true);
            var mockResolver = new Mock<IUserResolver>();
            var authManager = new AuthorizationManager(context, Mapper, mockResolver.Object);
            var directoryMnager = new DirectoryManager(context, Mapper, null);
            var repoManager = new RepositoryManager(context, Mapper, directoryMnager, authManager);
            var user = await SeedUserAsync(context);
            var owner = await SeedUserAsync(context);
            mockResolver.Setup(x => x.GetUserAsync()).Returns(Task.FromResult(Mapper.Map<UserDTO>(user)));
            var repoRoot = await repoManager.CreateRepositoryAsync(owner.Id, "Test Repo");

            //Act
            var repoFromDB = await context.Set<Repository>()
                .SingleOrDefaultAsync(x => x.RootId == repoRoot.Id);

            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(() => repoManager.DeleteRepositoryAsync(repoFromDB.Id));
        }
    }
}
