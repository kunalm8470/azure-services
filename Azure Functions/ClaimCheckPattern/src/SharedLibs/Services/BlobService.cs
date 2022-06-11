using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SharedLibs.Contracts;
using SharedLibs.Extensions;

namespace SharedLibs.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName, cancellationToken: cancellationToken);
        }

        public async Task<Stream> GetBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            return await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
        }

        public async Task<string> UploadBlobAsync(Stream readStream, string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            // Get the in memory reference to blobClient
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobHttpHeaders headers = new()
            {
                ContentType = fileName.GetContentType()
            };

            await blobClient.UploadAsync(readStream, headers, cancellationToken: cancellationToken);

            // Return the newly created resource
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
