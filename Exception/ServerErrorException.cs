using System.Runtime.Serialization;

namespace Azusa.Shared.Exception;

public class ServerErrorException : System.Exception
{
    public ServerErrorException() { }
    protected ServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ServerErrorException(string? message) : base(message) { }
    public ServerErrorException(string? message, System.Exception? innerException) : base(message, innerException) { }
}