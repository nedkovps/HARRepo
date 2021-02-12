using AutoMapper;
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
    public class DirectoryManager : IDirectoryManager
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileManager _fileManager;

        public DirectoryManager(DbContext context, IMapper mapper, IFileManager fileManager)
        {
            _context = context;
            _mapper = mapper;
            _fileManager = fileManager;
        }

        public async Task<DirectoryDTO> CreateDirectoryAsync(string name, int parentId)
        {
            var newDirectory = await _context.AddAsync(new Directory()
            {
                Name = name,
                ParentId = parentId
            });
            await _context.SaveChangesAsync();
            return _mapper.Map<DirectoryDTO>(newDirectory.Entity);
        }

        public async Task DeleteDirectoryAsync(int directoryId, bool isRoot = true, bool noTransaction = false)
        {
            using var transaction = isRoot && !noTransaction ? await _context.Database.BeginTransactionAsync() : null;
            var directory = await _context.Set<Directory>()
                .Where(x => x.Id == directoryId)
                .Include(x => x.SubDirectories)
                .Include(x => x.Files)
                .FirstOrDefaultAsync();
            if (directory != null)
            {
                if (directory.Files.Count > 0)
                {
                    await _fileManager.DeleteFilesAsync(directory.Files.ToList());
                }
                foreach (var subDirectory in directory.SubDirectories.ToList())
                {
                    await DeleteDirectoryAsync(subDirectory.Id, false);
                }
                _context.Remove(directory);
                await _context.SaveChangesAsync();
                if (isRoot && !noTransaction)
                {
                    await transaction.CommitAsync();
                }
            }
        }
    }
}
