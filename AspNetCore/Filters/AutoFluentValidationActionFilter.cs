using System.Reflection;
using Azusa.Shared.Exception;
using Azusa.Shared.ModelValidation.FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Azusa.Shared.AspNetCore.Filters;

/// <summary>
/// 自动校验筛选器，在操作方法开始前对标注了[AutoValidate]特性的操作方法参数使用FluentValidation进行校验
/// </summary>
public class AutoFluentValidationActionFilter : IAsyncActionFilter
{
    private readonly FluentIValidatorFactory _validatorFactory;

    public AutoFluentValidationActionFilter(FluentIValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor action)
        {
            var actionParams = action.Parameters;
            foreach (var descriptor in actionParams)
            {
                var paramInfo = (descriptor as ControllerParameterDescriptor)!.ParameterInfo;
                if (paramInfo.GetCustomAttribute<AutoFluentValidateAttribute>() is not null)
                {
                    var result = await _validatorFactory.ValidateAsync(paramInfo.ParameterType, context.ActionArguments[paramInfo.Name!]!);
                    if (!result.IsValid)
                        throw new ValidationErrorException(result);
                }
            }
        }


        await next();
    }
}