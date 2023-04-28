using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.AspNetCore.ApiResponses;

public class NotFoundApiResponse : ApiResponse
{
    public Type? Type { get; init; }
    
    public NotFoundApiResponse(string? message, Type? type) : base(StatusCodes.Status404NotFound,message)
    {
        Type = type;
    }
}