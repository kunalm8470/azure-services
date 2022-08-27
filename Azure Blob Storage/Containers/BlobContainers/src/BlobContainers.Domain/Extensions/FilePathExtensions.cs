using Microsoft.AspNetCore.StaticFiles;

namespace BlobContainers.Domain.Extensions;

public static class FilePathExtensions
{
    private const string GENERIC_FILE_CONTENT_TYPE = "application/octet-stream";

    public static string GetContentType(this string fileName)
    {
        if (fileName is null)
        {
            throw new ArgumentNullException("File name cannot be null", default(Exception)) ;
        }

        FileExtensionContentTypeProvider provider = new();

        return provider.TryGetContentType(fileName, out string contentType)
        ? contentType
        : GENERIC_FILE_CONTENT_TYPE;
    }
}

