using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Models
{
    public class FileUploadModel
    {
        public string Name { get; set; }

        public int DirectoryId { get; set; }

        public string Content { get; set; }
    }
}
