using System.Runtime.Serialization;

namespace Azusa.Shared.Exception;

/// <summary>
/// 出现程序逻辑错误
/// </summary>
public class ServerErrorException : System.Exception
{
    public ServerErrorException() : base("你请求的服务器发生了错误")
    { }
    protected ServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ServerErrorException(string? message) : base(message) { }
    public ServerErrorException(string? message, System.Exception? innerException) : base(message, innerException) { }
}