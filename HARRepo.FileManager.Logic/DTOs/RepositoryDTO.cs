using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.DTOs
{
    public class RepositoryDTO : BaseDTO
    {
        public string Name { get; set; }

        public int RootId { get; set; }

        public DateTime LastActivityOn { get; set; }
    }
}
