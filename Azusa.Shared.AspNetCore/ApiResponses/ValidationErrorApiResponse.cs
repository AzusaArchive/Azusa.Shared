using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.AspNetCore.ApiResponses;

/// <summary>
/// 带有数据校验错误信息的响应报文
/// </summary>
public class ValidationErrorApiResponse : ApiResponse
{
    public IList<ValidationResult> ValidationErrors { get; }
    public ValidationErrorApiResponse(string? message, IList<ValidationResult> validationErrors) : base(StatusCodes.Status406NotAcceptable ,message)
    {
        ValidationErrors = validationErrors;
    }
}