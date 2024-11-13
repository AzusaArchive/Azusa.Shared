using System.Reflection;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Azusa.Shared.DDD.EntityFramework
{
    /// <summary>
    /// 工作单元操作方法过滤器，将标注了UnitOfWorkAttribute特性的操作方法视为工作单元，
    /// 使用事务范围和DbContext实现事务最终一致性，DbContext注册的生命周期需要为Scope
    /// </summary>
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //不是Razor控制器
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            //获取操作方法特性
            var attr = descriptor.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();

            //操作方法结束后进行处理

            //未找到工作单元特性则正常执行操作方法
            if (attr == null)
            {
                await next();
                return;
            }

            //若有工作单元特性，使用事务范围覆盖操作方法
            //在包装有异步代码的事务中启用TransactionScopeAsyncFlowOption
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = await next();
            if (result.Exception != null)//若有未处理异常则放弃事务
                return;

            //从DI容器中获取响应的DbContext
            var serviceProvider = context.HttpContext.RequestServices;
            foreach (var dbContextType in attr.DbContextTypes)
            {
                var dbContext = (Microsoft.EntityFrameworkCore.DbContext)serviceProvider.GetRequiredService(dbContextType);
                await dbContext.SaveChangesAsync();
            }
            transaction.Complete();
        }
    }
}
