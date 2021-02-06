using Azure.Storage.Blobs;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Implementations
{
    public class AzureBlobFileStorageLogic : IFileStorageLogic
    {
        private readonly BlobContainerClient _client;

        public AzureBlobFileStorageLogic(IOptions<AzureBlobFileStorageOptions> options)
        {
            var blobClientOptions = options.Value;
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobClientOptions.ConnectionString);
            string containerName = blobClientOptions.ContainerName;
            _client = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadAsync(string content)
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();

            var name = Guid.NewGuid().ToString();
            await _client.UploadBlobAsync(name, stream);
            return name;
        }

        public async Task DeleteAsync(string path)
        {
            await _client.DeleteBlobIfExistsAsync(path);
        }
    }
}
