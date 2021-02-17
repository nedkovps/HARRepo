using HARRepo.FileManager.API.Models;
using HARRepo.FileManager.API.Validators;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Controllers
{
    [Route("api")]
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileManager _fileManager;
        private readonly IAuthorizationManager _auth;

        public FileController(IFileManager fileManagerLogic, IAuthorizationManager authorization)
        {
            _fileManager = fileManagerLogic;
            _auth = authorization;
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
            var validationResult = await new FileUploadValidator().ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var file = await _fileManager.UploadFileAsync(model.DirectoryId, model.Name, model.Content);
                return Ok(file);
            }
            else
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }
        }

        [HttpDelete("files/{fileId}")]
        public async Task<ActionResult> DeleteFileAsync(int fileId)
        {
            await _fileManager.DeleteFileAsync(fileId);
            return Ok();
        }

        [HttpGet("users/current/files/shared")]
        public async Task<ActionResult> GetUserSharedFiles()
        {
            var user = await _auth.GetCurrentUserAsync();
            var files = await _fileManager.GetUserSharedFilesAsync(user.Id);
            return Ok(files);
        }

        [HttpGet("users/current/files/sharedWith")]
        public async Task<ActionResult> GetFilesSharedWithUser()
        {
            var user = await _auth.GetCurrentUserAsync();
            var files = await _fileManager.GetFilesSharedWithUserAsync(user.Id);
            return Ok(files);
        }

        [HttpPost("files/share")]
        public async Task<ActionResult> ShareFile([FromBody] FileShareModel model)
        {
            var validationResult = await new FileShareValidator().ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var user = await _auth.GetUserByEmailAsync(model.UserEmail);
                var currentUser = await _auth.GetCurrentUserAsync();
                await _fileManager.ShareFileAsync(model.FileId, currentUser.Id, user.Id, model.Comment);
                return Ok();
            }
            else
            {
                return BadRequest(validationResult.ToErrorDictionary());
            }
        }

        [HttpDelete("files/unshare/{sharedFileId}")]
        public async Task<ActionResult> UnshareFile(int sharedFileId)
        {
            await _fileManager.UnshareFileAsync(sharedFileId);
            return Ok();
        }

        [HttpGet("users/current/files/sharedWith/count")]
        public async Task<ActionResult> GetSharedWithUserFilesCountAsync()
        {
            var user = await _auth.GetCurrentUserAsync();
            var sharedFilesCount = await _fileManager.GetSharedWithUserFilesCountAsync(user.Id);
            return Ok(sharedFilesCount);
        }
    }
}
