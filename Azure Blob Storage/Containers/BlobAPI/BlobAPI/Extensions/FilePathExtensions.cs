using Microsoft.AspNetCore.StaticFiles;

namespace BlobAPI.Extensions
{
    public static class FilePathExtensions
    {
        public static string GetContentType(this string fileName)
        {
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

            return provider.TryGetContentType(fileName, out var contentType)
            ? contentType
            : "application/octet-stream";
        }
    }
}
