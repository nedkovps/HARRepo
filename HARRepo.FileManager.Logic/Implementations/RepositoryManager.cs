﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Exceptions;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IDirectoryManager _directoryManager;
        private readonly IAuthorizationManager _auth;

        public RepositoryManager(DbContext context, IMapper mapper, IDirectoryManager directoryManager,
            IAuthorizationManager authorization)
        {
            _context = context;
            _mapper = mapper;
            _directoryManager = directoryManager;
            _auth = authorization;
        }

        public async Task<IList<RepositoryDTO>> GetUserRepositoriesAsync(int userId)
        {
            var userRepos = await _context.Set<Repository>()
                .Where(x => x.UserId == userId)
                .ProjectTo<RepositoryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return userRepos;
        }

        public async Task<RepositoryDetailsDTO> GetRepositoryAsync(int repoId)
        {
            var user = await _auth.GetCurrentUserAsync();
            var repo = await _context.Set<Repository>()
                .FindAsync(repoId);
            if (repo == null)
            {
                throw new NotFoundException();
            }
            if (repo.UserId != user.Id)
            {
                throw new AccessDeniedException();
            }
            var root = await _context.Set<Directory>()
                .FindAsync(repo.RootId);

            return new RepositoryDetailsDTO()
            {
                Id = repo.Id,
                Name = repo.Name,
                RootId = repo.RootId,
                Root = await FillDirectoryChildrenAsync(new DirectoryDTO()
                {
                    Id = root.Id,
                    Name = root.Name
                })
            };
        }

        private async Task<DirectoryDTO> FillDirectoryChildrenAsync(DirectoryDTO root)
        {
            var children = await _context.Set<Directory>()
                .Where(x => x.ParentId == root.Id)
                .ToListAsync();
            var files = await _context.Set<File>()
                .Where(x => x.DirectoryId == root.Id)
                .ToListAsync();

            var childrenDtos = children.Count > 0 ?
                children.Select(x =>
                {
                    var childDto = new DirectoryDTO()
                    {
                        Id = x.Id,
                        Name = x.Name
                    };
                    childDto = FillDirectoryChildrenAsync(childDto).Result;
                    return childDto;
                })
                .ToList()
                : new List<DirectoryDTO>();
            root.SubDirectories = childrenDtos;
            root.Files = files.Select(x => _mapper.Map<FileDTO>(x)).ToList();

            return root;
        }

        public async Task<DirectoryDTO> CreateRepositoryAsync(int userId, string name)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var root = await _context.AddAsync(new Directory()
            {
                Name = "Root"
            });
            await _context.SaveChangesAsync();
            var repo = await _context.AddAsync(new Repository()
            {
                UserId = userId,
                Name = name,
                RootId = root.Entity.Id,
                LastActivityOn = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return _mapper.Map<DirectoryDTO>(root.Entity);
        }

        public async Task DeleteRepositoryAsync(int repoId)
        {
            var user = await _auth.GetCurrentUserAsync();
            var repo = await _context.Set<Repository>()
                .FindAsync(repoId);
            if (repo == null)
            {
                throw new NotFoundException();
            }
            if (repo.UserId != user.Id)
            {
                throw new AccessDeniedException();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            _context.Remove(repo);
            await _context.SaveChangesAsync();
            await _directoryManager.DeleteDirectoryAsync(repo.RootId, noTransaction: true);
            await transaction.CommitAsync();
        }
    }
}
