using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IDirectoryManager
    {
        Task<DirectoryDTO> CreateDirectoryAsync(string name, int parentId);
        Task DeleteDirectoryAsync(int directoryId, bool isRoot = true, bool noTransaction = false);
    }
}
