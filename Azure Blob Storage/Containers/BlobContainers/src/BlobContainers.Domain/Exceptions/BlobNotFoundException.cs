using System.Runtime.Serialization;

namespace BlobContainers.Domain.Exceptions;

public class BlobNotFoundException : Exception
{
    public BlobNotFoundException()
    {
    }

    public BlobNotFoundException(string message) : base(message)
    {
    }

    public BlobNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BlobNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
