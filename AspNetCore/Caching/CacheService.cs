using Azusa.Shared.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;

namespace Azusa.Shared.AspNetCore.Caching;

public class CacheService
{
    protected readonly IDistributedCache _distributedCache;
    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
}