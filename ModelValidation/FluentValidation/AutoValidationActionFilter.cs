using System.Reflection;
using Azusa.Shared.Exception;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Azusa.Shared.ModelValidation.FluentValidation;

public class AutoValidationActionFilter : IAsyncActionFilter
{
    private readonly FluentIValidatorFactory _validatorFactory;

    public AutoValidationActionFilter(FluentIValidatorFactory validatorFactory)
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
                if (paramInfo.GetCustomAttribute<AutoValidateAttribute>() is not null)
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