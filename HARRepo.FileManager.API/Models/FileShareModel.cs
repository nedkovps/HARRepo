using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Models
{
    public class FileShareModel
    {
        public int FileId { get; set; }

        public string UserEmail { get; set; }

        public string Comment { get; set; }
    }
}
