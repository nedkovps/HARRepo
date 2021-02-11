using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IFileManager
    {
        Task UpdateFileLocationAsync(int fileId, int directoryId);
        Task<FileDTO> UploadFileAsync(int directoryId, string name, string content);
        Task DeleteFileAsync(int fileId);
        Task DeleteFilesAsync(List<File> files);
    }
}
