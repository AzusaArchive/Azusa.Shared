using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Azusa.Shared.Exception;

/// <summary>
/// 数据校验出现错误
/// </summary>
public class ValidationErrorException : System.Exception
{
    public IList<ValidationResult> ValidationErrors { get; init; } = new List<ValidationResult>();

    public ValidationErrorException():base("提交的数据出现校验错误") { }

    public ValidationErrorException(IList<ValidationResult> validationErrors):base("提交的数据出现校验错误") 
    {
        ValidationErrors = validationErrors;
    }

    public ValidationErrorException(FluentValidation.Results.ValidationResult fluentValidationResult) :
        base("提交的数据出现校验错误")
    {
        ValidationErrors = fluentValidationResult.Errors
            .Select(failures => new ValidationResult(failures.ErrorMessage, new[] { failures.PropertyName }))
            .ToArray();
    }
    public ValidationErrorException(string? message, IList<ValidationResult> validationErrors) : base(message)
    {
        ValidationErrors = validationErrors;
    }
    

    protected ValidationErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ValidationErrorException(string? message) : base(message) { }

    public ValidationErrorException(string? message, System.Exception? innerException) :
        base(message, innerException) { }
}