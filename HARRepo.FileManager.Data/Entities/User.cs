using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Repositories = new HashSet<Repository>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Repository> Repositories { get; private set; }
    }
}
