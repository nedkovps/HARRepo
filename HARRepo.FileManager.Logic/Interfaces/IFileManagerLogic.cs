using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IFileManagerLogic
    {
        Task<IList<RepositoryDTO>> GetUserRepositoriesAsync(int userId);
        Task<RepositoryDetailsDTO> GetRepositoryAsync(int repoId);
        Task<DirectoryDTO> CreateRepositoryAsync(int userId, string name);
        Task<DirectoryDTO> CreateDirectoryAsync(string name, int parentId);
        Task DeleteDirectoryAsync(int directoryId);
        Task UpdateFileLocationAsync(int fileId, int directoryId);
        Task<FileDTO> UploadFileAsync(int directoryId, string name, string content);
    }
}
