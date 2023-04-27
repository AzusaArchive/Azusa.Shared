using Microsoft.Extensions.Logging;

namespace Azusa.Shared.Exception;

public interface IAutoLogException
{
    //TODO:为异常加上日志
    public LogLevel LogLevel { get; init; }
    public void Log(ILogger logger);
}