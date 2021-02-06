using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IFileStorageLogic
    {
        Task<string> UploadAsync(string content);
        Task DeleteAsync(string path);
    }
}
