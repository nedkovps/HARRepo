using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IFileManagerLogic
    {
        Task<DirectoryDTO> GetUserRootDirectoryAsync(int userId);

        Task UploadFile();

        Task UpdateFileLocation(int fileId, int directoryId);
    }
}
