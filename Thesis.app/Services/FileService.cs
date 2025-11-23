using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Thesis.app.Services
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string path);
        Task DeleteAsync(string blobPath);
        string GetFileUrl(string blobPath);
    }
    public class FileService  : IFileService
    {
        private readonly BlobContainerClient containerClient;
            
        public FileService(IOptions<AzureStorageOptions> options)
        {
            var config = options.Value;

            containerClient = new BlobContainerClient(config.ConnectionString, config.ContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }
            
        public async Task<string> UploadAsync(IFormFile file, string path)
        {
            var blobClient = containerClient.GetBlobClient(path);

            await using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

            return blobClient.Uri.ToString();
        }

        public async Task DeleteAsync(string blobPath)
        {
            var blobName = blobPath.Split('/').Last();
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        public string GetFileUrl(string blobPath)
        {
            var blobClient = containerClient.GetBlobClient(blobPath);

            return blobClient.Uri.ToString();
        }

    }
}
