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
            type.IsAssignableTo(typeof(IService)) && (type.IsInterface || type.IsAbstract)));
        foreach (var impl in implementationAssembly.GetTypes())
        {
            if (impl.IsAssignableTo(typeof(IService)) && impl.IsClass)
            {   
                //服务的声明周期首先取决于抽象实现的生命周期接口(ITransientService,IScopedService,ISingletonService)。
                //如果服务没有对应的抽象，则取决于服务实现的生命周期接口
                //默认为Scoped

                //所有可实现的抽象
                var types = abstractions.Where(type => impl.IsAssignableTo(type)).ToArray();
                foreach (var abs in types)
                {
                    if (abs.IsAssignableTo(typeof(ITransientService)))
                        services.AddTransient(abs, impl);
                    else if (abs.IsAssignableTo(typeof(ISingletonService)))
                        services.AddSingleton(abs, impl);
                    else
                        services.AddScoped(abs, impl);
                    Debug.WriteLine($"添加服务：<{abs.Name}> <{impl.Name}>");
                }

                //没有可实现的抽象
                if (!types.Any())
                {
                    if (impl.IsAssignableTo(typeof(ITransientService)))
                        services.AddTransient(impl);
                    else if (impl.IsAssignableTo(typeof(ISingletonService)))
                        services.AddSingleton(impl);
                    else
                        services.AddScoped(impl);
                    
                    Debug.WriteLine($"添加服务：<{impl.Name}>");
                }
            }
        }

        return services;
    }
}