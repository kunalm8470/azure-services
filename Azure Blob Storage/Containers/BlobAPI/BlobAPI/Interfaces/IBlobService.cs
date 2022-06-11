using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobAPI.Interfaces
{
    public interface IBlobService
    {
        public Task<BlobDownloadResult> GetBlobAsync(string containerName, string blobName);
        public Task<IEnumerable<string>> ListBlobsAsync(string containerName);
        public Task UploadBlobAsync(IFormFile file, string containerName);
        public Task DeleteBlobAsync(string containerName, string blobName);
    }
}
