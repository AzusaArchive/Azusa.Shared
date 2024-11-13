using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.AspNetCore.ApiResponses
{
    /// <summary>
    /// 通用的响应报文模型
    /// </summary>
    public class ApiResponse
    {
        public int Code { get; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string? Message { get; }

        public ApiResponse(int code, string? message)
        {
            Code = code;
            Message = message;
        }

        public static ApiResponse Ok(string? message = "操作成功")
        {
            return new ApiResponse(StatusCodes.Status200OK, message);
        }

        public static ObjectApiResponse<TResult> Ok<TResult>(TResult data, string? msg = "操作成功",
            int code = StatusCodes.Status200OK)
        {
            return new ObjectApiResponse<TResult>(code, msg, data);
        }

        public static ObjectApiResponse<TResult> Created<TResult>(TResult data, string? url, string? msg = "创建成功")
        {
            return new CreatedApiResponse<TResult>(msg, data, url);
        }

        public static ApiResponse BadRequest(string? message = "请求失败")
        {
            return new ApiResponse(StatusCodes.Status400BadRequest, message);
        }

        public static ValidationErrorApiResponse ValidationError(IList<ValidationResult> errors, string? msg = "数据校验错误")
        {
            return new ValidationErrorApiResponse(msg, errors);
        }

        public static ApiResponse NotFound(Type? type, string? msg = null)
        {
            return new ApiResponse(StatusCodes.Status404NotFound, $"找不到请求的{type?.Name}对象");
        }
    }
}