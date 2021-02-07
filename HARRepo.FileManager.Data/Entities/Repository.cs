using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Entities
{
    public class Repository : BaseEntity
    {
        public string Name { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; private set; }

        public int RootId { get; set; }
        public virtual Directory Root { get; private set; }

        public DateTime LastActivityOn { get; set; }
    }
}
