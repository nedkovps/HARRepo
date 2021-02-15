using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IAuthorizationManager
    {
        Task<UserDTO> GetCurrentUserAsync();
    }
}
