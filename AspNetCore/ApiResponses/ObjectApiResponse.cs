namespace Azusa.Shared.AspNetCore.ApiResponses;

/// <summary>
/// 带有数据模型的响应报文
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class ObjectApiResponse<TResult> : ApiResponse
{
    /// <summary>
    /// 响应的数据
    /// </summary>
    public TResult Data { get; }

    public ObjectApiResponse(int code, string? message, TResult data) : base(code, message)
    {
        Data = data;
    }
}