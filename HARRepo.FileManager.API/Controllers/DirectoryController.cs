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
    public class DirectoryController : Controller
    {
        private readonly IDirectoryManager _dirManager;

        public DirectoryController(IDirectoryManager directoryManager)
        {
            _dirManager = directoryManager;
        }

        [HttpPost("directories")]
        public async Task<ActionResult> CreateDirectory(string name, int parentId)
        {
            var directory = await _dirManager.CreateDirectoryAsync(name, parentId);
            return Ok(directory);
        }

        [HttpDelete("directories/{directoryId}")]
        public async Task<ActionResult> DeleteDirectory(int directoryId)
        {
            await _dirManager.DeleteDirectoryAsync(directoryId);
            return Ok();
        }
    }
}
