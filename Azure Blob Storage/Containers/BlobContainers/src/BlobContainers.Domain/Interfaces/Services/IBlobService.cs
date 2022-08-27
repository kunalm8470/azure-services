using Azure.Storage.Blobs.Models;

namespace BlobContainers.Domain.Interfaces.Services;

public interface IBlobService
{
    public Task<BlobDownloadResult> GetBlobAsync(string containerName, string blobName, CancellationToken token = default);
    public Task<IEnumerable<string>> ListBlobsAsync(string containerName, CancellationToken token = default);
    public Task UploadBlobAsync(Stream readStream, string blobName, string containerName, CancellationToken token = default);
    public Uri GenerateSASUrlAsync(string accountName, string accountKey, string containerName, string blobName, int expiresInMinutes = 20);
    public Task DeleteBlobAsync(string containerName, string blobName, CancellationToken token = default);
}
