using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.DTOs
{
    public class SharedFileDTO : BaseDTO
    {
        public int FileId { get; set; }
        public FileDTO File { get; set; }

        public int SharedWithId { get; set; }
        public UserDTO SharedWith { get; set; }

        public string Comment { get; set; }
    }
}
