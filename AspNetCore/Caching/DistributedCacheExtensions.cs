using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Azusa.Shared.AspNetCore.Caching;

public static  class DistributedCacheExtensions
{
    /// <summary>
    /// 获取或创建缓存
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key">键</param>
    /// <param name="factory">缓存值工厂函数</param>
    /// <param name="expireMinutes">过期时间（分）</param>
    /// <typeparam name="TCacheItem">缓存值的类型</typeparam>
    /// <returns>缓存值</returns>
    /// <exception cref="InvalidCastException">当获取到的缓存无法序列化为指定的类型时</exception>
    public static async Task<TCacheItem> GetOrCreateAsync<TCacheItem>(this IDistributedCache cache, string key,
        Func<Task<TCacheItem>> factory, int expireMinutes = 60) 
    {
        var value = await cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<TCacheItem>(value) ?? throw new InvalidCastException($"无法将获取的字符串转换为{typeof(TCacheItem)}，字符串：{value}");

        var item = await factory.Invoke();

        var entry = new DistributedCacheEntryOptions();
        if (expireMinutes != -1)
            entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expireMinutes);

        await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(item), entry);
        return item;
    }
}