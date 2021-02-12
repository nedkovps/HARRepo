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
                _context.RemoveRange(removedFiles);
                await _context.SaveChangesAsync();
            }
        }
    }
}
