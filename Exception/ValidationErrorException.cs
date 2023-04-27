using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Azusa.Shared.Exception;

public class ValidationErrorException : System.Exception
{
    public IList<ValidationResult> ValidationErrors { get; init; } = new List<ValidationResult>();

    public ValidationErrorException() { }

    public ValidationErrorException(IList<ValidationResult> validationErrors)
    {
        ValidationErrors = validationErrors;
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