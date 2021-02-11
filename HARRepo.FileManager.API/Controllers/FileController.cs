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
    public class FileController : Controller
    {
        private readonly IFileManager _fileManager;

        public FileController(IFileManager fileManagerLogic)
        {
            _fileManager = fileManagerLogic;
        }

        [HttpPut("files/{fileId}/directory/{directoryId}")]
        public async Task<ActionResult> UpdateFileLocation(int fileId, int directoryId)
        {
            await _fileManager.UpdateFileLocationAsync(fileId, directoryId);
            return Ok();
        }

        [HttpPost("files")]
        public async Task<ActionResult> UploadFile([FromBody]FileUploadModel model)
        {
            var file = await _fileManager.UploadFileAsync(model.DirectoryId, model.Name, model.Content);
            return Ok(file);
        }

        [HttpDelete("files/{fileId}")]
        public async Task<ActionResult> DeleteFileAsync(int fileId)
        {
            await _fileManager.DeleteFileAsync(fileId);
            return Ok();
        }
    }
}
