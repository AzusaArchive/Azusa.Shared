using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.AspNetCore.ApiResponses;

public class CreatedApiResponse<TResult> : ObjectApiResponse<TResult>
{
    public string? Url { get; init; }
    public CreatedApiResponse(string? message, TResult data, string? url) : base(StatusCodes.Status201Created, message, data)
    {
        Url = url;
    }
}