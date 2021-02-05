using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Entities
{
    public class Directory : BaseEntity
    {
        public Directory()
        {
            SubDirectories = new HashSet<Directory>();
            Files = new HashSet<File>();
        }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public virtual Directory Parent { get; private set; }

        public ICollection<Directory> SubDirectories { get; private set; }
        public ICollection<File> Files { get; private set; }
    }
}
