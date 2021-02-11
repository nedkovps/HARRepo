using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IRepositoryManager
    {
        Task<IList<RepositoryDTO>> GetUserRepositoriesAsync(int userId);
        Task<RepositoryDetailsDTO> GetRepositoryAsync(int repoId);
        Task<DirectoryDTO> CreateRepositoryAsync(int userId, string name);
    }
}
