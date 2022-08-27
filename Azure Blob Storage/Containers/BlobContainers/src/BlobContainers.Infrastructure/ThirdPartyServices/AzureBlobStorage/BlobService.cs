using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using BlobContainers.Domain.Exceptions;
using BlobContainers.Domain.Extensions;
using BlobContainers.Domain.Interfaces.Services;

namespace BlobContainers.Infrastructure.ThirdPartyServices.AzureBlobStorage;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task DeleteBlobAsync(string containerName, string blobName, CancellationToken token = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken: token))
        {
            throw new BlobNotFoundException("Blob doesn't exist!");
        }

        await blobClient.DeleteAsync(cancellationToken: token);
    }

    public Uri GenerateSASUrlAsync(string accountName, string accountKey, string containerName, string blobName, int expiresInMinutes = 20)
    {
        BlobSasBuilder blobSasBuilder = new()
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b", //b = blob, c = container
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiresInMinutes)
        };

        blobSasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

        StorageSharedKeyCredential storageSharedKeyCredential = new(accountName, accountKey);

        string sasToken = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

        UriBuilder fullUri = new()
        {
            Scheme = "https",
            Host = string.Format("{0}.blob.core.windows.net", accountName),
            Path = string.Format("{0}/{1}", containerName, blobName),
            Query = sasToken
        };

        return fullUri.Uri;
    }

    public async Task<BlobDownloadResult> GetBlobAsync(string containerName, string blobName, CancellationToken token = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken: token))
        {
            throw new BlobNotFoundException("Blob doesn't exist!");
        }

        return await blobClient.DownloadContentAsync(cancellationToken: token);
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(string containerName, CancellationToken token = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        List<string> blobs = new();

        await foreach (BlobItem item in containerClient.GetBlobsAsync(cancellationToken: token))
        {
            blobs.Add(item.Name);
        }

        return blobs;
    }

    public async Task UploadBlobAsync(Stream readStream, string blobName, string containerName, CancellationToken token = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        // Get the in memory reference to blobClient
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(readStream, new BlobHttpHeaders
        {
            ContentType = blobName.GetContentType()
        }, cancellationToken: token);
    }
}
