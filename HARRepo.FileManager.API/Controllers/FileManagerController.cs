using HARRepo.FileManager.API.Models;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Controllers
{
    [Route("api")]
    public class FileManagerController : Controller
    {
        private readonly IFileManagerLogic _fileManager;

        public FileManagerController(IFileManagerLogic fileManagerLogic)
        {
            _fileManager = fileManagerLogic;
        }

        [HttpGet("users/{userId}/repositories")]
        public async Task<ActionResult> GetUserRepositories(int userId)
        {
            var repos = await _fileManager.GetUserRepositoriesAsync(userId);
            return Ok(repos);
        }

        [HttpGet("repositories/{repoId}")]
        public async Task<ActionResult> GetRepositoryRoot(int repoId)
        {
            var root = await _fileManager.GetRepositoryRootAsync(repoId);
            return Ok(root);
        }

        [HttpPost("directories")]
        public async Task<ActionResult> CreateDirectory(string name, int parentId)
        {
            var directory = await _fileManager.CreateDirectoryAsync(name, parentId);
            return Ok(directory);
        }

        [HttpDelete("directories/{directoryId}")]
        public async Task<ActionResult> DeleteDirectory(int directoryId)
        {
            await _fileManager.DeleteDirectoryAsync(directoryId);
            return Ok();
        }

        [HttpPut("files/{fileId}/directory/{directoryId}")]
        public async Task<ActionResult> UpdateFileLocation(int fileId, int directoryId)
        {
            await _fileManager.UpdateFileLocationAsync(fileId, directoryId);
            return Ok();
        }

        [HttpPost("files")]
        public async Task<ActionResult> UploadFile(FileUploadModel model)
        {
            var file = await _fileManager.UploadFileAsync(model.DirectoryId, model.Name, model.Content);
            return Ok(file);
        }
    }
}
