using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Implementations
{
    public class AzureBlobFileStorageOptions
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
