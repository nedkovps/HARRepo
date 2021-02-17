using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Entities
{
    public class SharedFile : BaseEntity
    {
        public int FileId { get; set; }
        public File File { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public int SharedWithId { get; set; }
        public User SharedWith { get; set; }

        public string Comment { get; set; }
    }
}
