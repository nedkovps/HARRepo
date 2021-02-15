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
    public class RepositoryController : Controller
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IAuthorizationManager _auth;

        public RepositoryController(IRepositoryManager repositoryManager, IAuthorizationManager authorization)
        {
            _repoManager = repositoryManager;
            _auth = authorization;
        }

        [HttpGet("users/current/repositories")]
        public async Task<ActionResult> GetUserRepositories()
        {
            var user = await _auth.GetCurrentUserAsync();
            var repos = await _repoManager.GetUserRepositoriesAsync(user.Id);
            return Ok(repos);
        }

        [HttpGet("repositories/{repoId}")]
        public async Task<ActionResult> GetRepository(int repoId)
        {
            var repo = await _repoManager.GetRepositoryAsync(repoId);
            return Ok(repo);
        }

        [HttpPost("users/current/repositories")]
        public async Task<ActionResult> CreateRepository(string name)
        {
            var user = await _auth.GetCurrentUserAsync();
            var repoRoot = await _repoManager.CreateRepositoryAsync(user.Id, name);
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
