using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Azusa.Shared.AspNetCore.ApiResponses;

public class ValidationErrorApiResponse : ApiResponse
{
    public IList<ValidationResult> ValidationErrors { get; }
    public ValidationErrorApiResponse(string? message, IList<ValidationResult> validationErrors) : base(StatusCodes.Status406NotAcceptable ,message)
    {
        ValidationErrors = validationErrors;
    }
}