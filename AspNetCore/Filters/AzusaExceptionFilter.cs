using Azusa.Shared.AspNetCore.ApiResponses;
using Azusa.Shared.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Azusa.Shared.AspNetCore.Filters;

public class AzusaExceptionFilter : IExceptionFilter
{
    private readonly ILogger<AzusaExceptionFilter> _logger;
    public AzusaExceptionFilter(ILogger<AzusaExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (!context.ExceptionHandled)
        {
            //TODO:改为在异常内执行，并有各自的日志级别
            _logger.LogInformation(context.Exception, "异常过滤器捕获： ");

            IActionResult result = context.Exception switch
            {
                EntityNotFoundException e => new ObjectResult(ApiResponse.NotFound(e.EntityType!,e.Message)){StatusCode = StatusCodes.Status404NotFound},
                ServerErrorException e => new ObjectResult(new ApiResponse(500,e.Message)){StatusCode = StatusCodes.Status500InternalServerError},
                UserUnauthorizedException e => new ObjectResult(new ApiResponse(401,e.Message)){StatusCode = StatusCodes.Status401Unauthorized},
                ValidationErrorException e => new ObjectResult(ApiResponse.ValidationError(e.ValidationErrors,e.Message)){StatusCode = StatusCodes.Status406NotAcceptable},
                _ => new ObjectResult(new ApiResponse(StatusCodes.Status501NotImplemented,"请求的服务器发生了错误")){StatusCode = 501}
            };
            context.Result = result;
            context.ExceptionHandled = true;
#if DEBUG
            if (context.Exception is ServerErrorException || result is ObjectResult { StatusCode: 501 or 500 })
            {
                throw context.Exception;
            }
#endif
        }
    }
}