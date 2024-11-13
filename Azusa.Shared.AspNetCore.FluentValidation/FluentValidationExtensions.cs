using System.ComponentModel.DataAnnotations;
using Azusa.Shared.Exception;
using FluentValidation;

namespace Azusa.Shared.ModelValidation.FluentValidation;

public static class FluentValidationExtensions
{
    /// <summary>
    /// 对FluentValidation的ValidateAndThrowAsync方法进行封装，抛出我自定义的异常。
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="input"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ValidationErrorException"></exception>
    public static async Task AzusaValidateAndThrowAsync<T>(this IValidator<T> validator, T input)
    {
        var result = await validator.ValidateAsync(input);
        if (!result.IsValid)
            throw new ValidationErrorException(result.Errors
                .Select(failures => new ValidationResult(failures.ErrorMessage, new []{failures.PropertyName}))
                .ToArray());
    }
}