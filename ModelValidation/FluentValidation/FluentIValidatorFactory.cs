using System.Diagnostics.CodeAnalysis;
using Azusa.Shared.Exception;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Azusa.Shared.ModelValidation.FluentValidation;

/// <summary>
/// 针对FluentValidation中的IValidator&lt;&gt;接口的工厂类，通过ServiceProvider直接获取IValidator&lt;&gt;，或是使用System.Type动态地对模型进行校验
/// </summary>
public class FluentIValidatorFactory
{
    private readonly IServiceProvider _services;
    public FluentIValidatorFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IValidator<TModel>? GetGenericValidator<TModel>()
    {
        return _services.GetService(typeof(IValidator<TModel>)) as IValidator<TModel>;
    }

    public Task<ValidationResult> ValidateAsync(Type modelType, object model)
    {
        var genericType = typeof(IValidator<>).MakeGenericType(modelType);
        var validator = _services.GetService(genericType);
        if (validator is null)
            throw new ServerErrorException($"没有找到[IValidator<{modelType.Name}>]类型的服务，检查你的依赖注入");
        var methodInfo = genericType.GetMethod(nameof(IValidator<object>.ValidateAsync));
        if (methodInfo is null)
            throw new ServerErrorException($"无法获取到[IValidator<{modelType.Name}>.ValidateAsync]方法，");
        return (Task<ValidationResult>)methodInfo.Invoke(validator, new []{model, (CancellationToken)default})!;
    }
}