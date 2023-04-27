﻿using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.WebAPI.ApiResponses;

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