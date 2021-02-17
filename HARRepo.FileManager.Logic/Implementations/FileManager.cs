using AutoMapper;
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
    public class FileManager : IFileManager
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public FileManager(DbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        public async Task UpdateFileLocationAsync(int fileId, int directoryId)
        {
            var file = await _context.Set<File>()
                .FindAsync(fileId);
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
            return _mapper.Map<FileDTO>(newFile.Entity);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _context.Set<File>()
                .FindAsync(fileId);
            if (file != null)
            {
                await _fileStorage.DeleteAsync(file.Path);
                var sharedFiles = _context.Set<SharedFile>()
                    .Where(x => x.FileId == fileId);
                _context.RemoveRange(sharedFiles);
                _context.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFilesAsync(List<File> files)
        {
            List<File> removedFiles = new List<File>();
            try
            {
                foreach (var file in files)
                {
                    await _fileStorage.DeleteAsync(file.Path);
                    removedFiles.Add(file);
                }
            }
            finally
            {
                var fileIds = removedFiles.Select(x => x.Id).ToList();
                var sharedFiles = _context.Set<SharedFile>()
                    .Where(x => fileIds.Contains(x.FileId));
                _context.RemoveRange(sharedFiles);
                _context.RemoveRange(removedFiles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SharedFileDTO>> GetUserSharedFilesAsync(int userId)
        {
            var files = await _context.Set<SharedFile>()
                .Where(x => x.OwnerId == userId)
                .ProjectTo<SharedFileDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return files;
        }

        public async Task<List<SharedWithMeFileDTO>> GetFilesSharedWithUserAsync(int userId)
        {
            var files = await _context.Set<SharedFile>()
                .Where(x => x.SharedWithId == userId)
                .ProjectTo<SharedWithMeFileDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return files;
        }

        public async Task ShareFileAsync(int fileId, int ownerId, int userId, string comment)
        {
            var file = await _context.Set<File>()
                .FindAsync(fileId);
            if (file == null)
            {
                throw new NotFoundException();
            }
            var existingSharedFile = await _context.Set<SharedFile>()
                .SingleOrDefaultAsync(x => x.FileId == fileId && x.OwnerId == ownerId && x.SharedWithId == userId);
            if (existingSharedFile != null)
            {
                throw new LogicException("File is already shared with specified user.");
            }
            await _context.AddAsync(new SharedFile() 
            { 
                FileId = file.Id,
                OwnerId = ownerId,
                SharedWithId = userId,
                Comment = comment
            });
            await _context.SaveChangesAsync();
        }

        public async Task UnshareFileAsync(int sharedFileId)
        {
            var sharedFile = await _context.Set<SharedFile>()
                .FindAsync(sharedFileId);
            if (sharedFile == null)
            {
                throw new NotFoundException();
            }
            _context.Remove(sharedFile);
            await _context.SaveChangesAsync();
        }
    }
}
