using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.DTOs
{
    public class RepositoryDetailsDTO : RepositoryDTO
    {
        public DirectoryDTO Root { get; set; }
    }
}
