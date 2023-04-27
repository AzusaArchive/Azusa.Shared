using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Azusa.Shared.WebAPI.ApiResponses
{
    public class ApiResponse
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string? Message { get; }

        public ApiResponse(string code, string? message)
        {
            Code = code;
            Message = message;
        }

        public static ApiResponse Ok(string? message = "操作成功")
        {
            return new ApiResponse(ApiResponseCode.Ok, message);
        }

        public static ObjectApiResponse<TResult> Ok<TResult>(TResult data, string? msg = "操作成功")
        {
            return new ObjectApiResponse<TResult>(ApiResponseCode.Ok, msg, data);
        }

        public static ApiResponse BadRequest(string? message = null)
        {
            return new ApiResponse(ApiResponseCode.BadRequest, message);
        }

        public static ValidationErrorApiResponse ValidationError(IList<ValidationResult> errors, string? msg = null)
        {
            return new ValidationErrorApiResponse(ApiResponseCode.ValidationError, msg, errors);
        }
    }
}