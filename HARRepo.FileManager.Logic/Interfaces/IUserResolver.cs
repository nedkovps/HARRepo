using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Interfaces
{
    public interface IUserResolver
    {
        Task<UserDTO> GetUserAsync();

        string GetUserId();
    }
}
