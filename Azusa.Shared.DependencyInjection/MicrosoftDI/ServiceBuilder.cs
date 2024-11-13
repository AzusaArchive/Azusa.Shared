using Microsoft.Extensions.DependencyInjection;

namespace Azusa.Shared.DependencyInjection.MicrosoftDI;

public class ServiceBuilder
{
    public IServiceCollection Services { get; init; }

    public ServiceBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public ForServiceBuilder For<TAbstraction>() => For(typeof(TAbstraction));

    public ForServiceBuilder For(Type abstractionType)
    {
        return new ForServiceBuilder(abstractionType, this);
    }
}

public class ForServiceBuilder : ServiceBuilder
{
    public Type ForType { get; init; }

    public ForServiceBuilder(Type forType, ServiceBuilder builder) : base(builder.Services)
    {
        ForType = forType;
    }

    public ServiceBuilder Use<TImplement>()
    {
        return Use(typeof(TImplement));
    }

    public ServiceBuilder Use(Type implementation)
    {
        Services.AddByLifeTime(ForType, implementation);
        return this;
    }

    public ServiceBuilder Use(Func<Type> factory) => Use(factory());
}