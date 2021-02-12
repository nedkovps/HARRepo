using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Controllers
{
    [Route("api")]
    public class RepositoryController : Controller
    {
        private readonly IRepositoryManager _repoManager;

        public RepositoryController(IRepositoryManager repositoryManager)
        {
            _repoManager = repositoryManager;
        }

        [HttpGet("users/{userId}/repositories")]
        public async Task<ActionResult> GetUserRepositories(int userId)
        {
            var repos = await _repoManager.GetUserRepositoriesAsync(userId);
            return Ok(repos);
        }

        [HttpGet("repositories/{repoId}")]
        public async Task<ActionResult> GetRepository(int repoId)
        {
            var repo = await _repoManager.GetRepositoryAsync(repoId);
            return Ok(repo);
        }

        [HttpPost("users/{userId}/repositories")]
        public async Task<ActionResult> CreateRepository(int userId, string name)
        {
            var repoRoot = await _repoManager.CreateRepositoryAsync(userId, name);
            return Ok(repoRoot);
        }
        
        [HttpDelete("repositories/{id}")]
        public async Task<ActionResult> DeleteRepository(int id)
        {
            await _repoManager.DeleteRepositoryAsync(id);
            return Ok();
        }
    }
}
