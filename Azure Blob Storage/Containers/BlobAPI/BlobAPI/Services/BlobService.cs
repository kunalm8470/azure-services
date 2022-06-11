using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobAPI.Extensions;
using BlobAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobAPI.Services
{
    public class BlobService : IBlobService
    {
        private readonly ILogger<BlobService> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(ILogger<BlobService> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }

        public async Task<BlobDownloadResult> GetBlobAsync(string containerName, string blobName)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                return await blobClient.DownloadContentAsync().ConfigureAwait(false);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Get blob failed because: {0}", ex.Message);
                return default(BlobDownloadResult);
            }
        }

        public async Task<IEnumerable<string>> ListBlobsAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            List<string> blobs = new List<string>();

            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                blobs.Add(item.Name);
            }

            return blobs;
        }

        public async Task UploadBlobAsync(IFormFile file, string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            // Get the in memory reference to blobClient
            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
            {
                ContentType = file.FileName.GetContentType()
            }).ConfigureAwait(false);
        }
    }
}
