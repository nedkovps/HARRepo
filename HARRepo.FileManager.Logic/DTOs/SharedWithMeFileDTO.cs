using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.DTOs
{
    public class SharedWithMeFileDTO : BaseDTO
    {
        public int FileId { get; set; }
        public FileDTO File { get; set; }

        public int SharedById { get; set; }
        public UserDTO SharedBy { get; set; }

        public string Comment { get; set; }
    }
}
