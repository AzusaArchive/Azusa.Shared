using System.ComponentModel.DataAnnotations;

namespace Azusa.Shared.WebAPI.ApiResponses;

public class ValidationErrorApiResponse : ApiResponse
{
    public IList<ValidationResult> ValidationErrors { get; }
    public ValidationErrorApiResponse(string code, string? message, IList<ValidationResult> validationErrors) : base(code, message)
    {
        ValidationErrors = validationErrors;
    }
}