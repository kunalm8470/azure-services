namespace SharedLibs.Contracts
{
    public interface IBlobService
    {
        public Task<Stream> GetBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
        public Task<string> UploadBlobAsync(Stream readStream, string fileName, string containerName, CancellationToken cancellationToken = default);
        public Task DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    }
}
