using Microsoft.Extensions.Caching.Distributed;

namespace Azusa.Shared.AspNetCore.Caching;

public class CacheService
{
    protected readonly IDistributedCache DistributedCache;
    public CacheService(IDistributedCache distributedCache)
    {
        DistributedCache = distributedCache;
    }
}