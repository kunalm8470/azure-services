using Microsoft.AspNetCore.StaticFiles;

namespace SharedLibs.Extensions
{
    public static class FilePathExtensions
    {
        public static string GetContentType(this string fileName)
        {
            FileExtensionContentTypeProvider provider = new();

            return provider.TryGetContentType(fileName, out var contentType)
            ? contentType
            : "application/octet-stream";
        }


    }
}
