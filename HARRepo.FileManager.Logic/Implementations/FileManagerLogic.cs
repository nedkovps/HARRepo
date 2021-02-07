using AutoMapper;
using AutoMapper.QueryableExtensions;
using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Implementations
{
    public class FileManagerLogic : IFileManagerLogic
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageLogic _fileStorage;

        public FileManagerLogic(DbContext context, IMapper mapper, IFileStorageLogic fileStorage)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        public async Task<IList<RepositoryDTO>> GetUserRepositoriesAsync(int userId)
        {
            var userRepos = await _context.Set<Repository>()
                .Where(x => x.UserId == userId)
                .ProjectTo<RepositoryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return userRepos;
        }

        public async Task<DirectoryDTO> GetRepositoryRootAsync(int repoId)
        {
            var root = await _context.Set<Repository>()
                .Where(x => x.Id == repoId)
                .Select(x => x.Root)
                .SingleOrDefaultAsync();

            return _mapper.Map<DirectoryDTO>(root);
        }

        public async Task<DirectoryDTO> CreateRepositoryAsync(int userId, string name)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var root  = await _context.AddAsync(new Directory()
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
            return _mapper.Map<DirectoryDTO>(root);
        }

        public async Task<DirectoryDTO> CreateDirectoryAsync(string name, int parentId)
        {
            var newDirectory = await _context.AddAsync(new Directory() 
            {
                Name = name,
                ParentId = parentId
            });
            await _context.SaveChangesAsync();
            return _mapper.Map<DirectoryDTO>(newDirectory);
        }

        public async Task DeleteDirectoryAsync(int directoryId)
        {
            var directory = await _context.Set<Directory>()
                .Where(x => x.Id == directoryId)
                .Include(x => x.SubDirectories)
                .Include(x => x.Files)
                .FirstOrDefaultAsync();
            if (directory != null)
            {
                //TODO: delete all files and folders under current directory
            }
        }

        public async Task UpdateFileLocationAsync(int fileId, int directoryId)
        {
            var file = await _context.Set<File>()
                .SingleOrDefaultAsync();
            if (file != null)
            {
                file.DirectoryId = directoryId;
                _context.Update(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FileDTO> UploadFileAsync(int directoryId, string name, string content)
        {
            var path = await _fileStorage.UploadAsync(content);
            var newFile = await _context.AddAsync(new File()
            { 
                Name = name,
                Path = path,
                DirectoryId = directoryId
            });
            await _context.SaveChangesAsync();
            return _mapper.Map<FileDTO>(newFile);
        }
    }
}
