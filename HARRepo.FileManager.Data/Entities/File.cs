using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Entities
{
    public class File : BaseEntity
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int DirectoryId { get; set; }
        public virtual Directory Directory { get; private set; }
    }
}
