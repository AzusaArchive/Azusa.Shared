using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Azusa.Shared.DDD.Application.Abstractions.Basic;
using Microsoft.Extensions.DependencyInjection;

namespace Azusa.Shared.DependencyInjection.MicrosoftDI;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// <para>在程序集中搜索AppService的抽象以及实现并加入DI</para>
    /// <para>对AppService的定义是实现IApplicationService接口</para>
    /// <para>对抽象的定义是抽象类或是接口</para>
    /// <para>对实现的定义是集成实现了抽象的类</para>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="abstractionAssembly">抽象所在的程序集</param>
    /// <param name="implementationAssembly">实现所在的程序集</param>
    /// <returns></returns>
    public static IServiceCollection AddAppServiceFromAssembly(this IServiceCollection services, Assembly abstractionAssembly, Assembly implementationAssembly)
    {
        var abstractions = new HashSet<Type>(abstractionAssembly.GetTypes().Where(type =>
            type.IsAssignableTo(typeof(IApplicationService)) && (type.IsInterface || type.IsAbstract)));
        foreach (var impl in implementationAssembly.GetTypes())
        {
            if (impl.IsAssignableTo(typeof(IApplicationService)))
            {
                //所有可实现的抽象
                var types = abstractions.Where(type => impl.IsAssignableTo(type)).ToArray();
                foreach (var abs in types)
                {
                    services.AddScoped(abs, impl);
                    Debug.WriteLine($"添加服务：<{abs.Name}> <{impl.Name}>");
                }

                if (!types.Any())
                {
                    services.AddScoped(impl);
                    Debug.WriteLine($"添加服务：<{impl.Name}>");
                }
            }
        }

        return services;
    }
}