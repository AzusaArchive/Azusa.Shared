namespace Azusa.Shared.AspNetCore.Caching;

/// <summary>
/// 缓存过期时间 单位：分钟
/// </summary>
public class CacheExpireTime
{
    /// <summary>
    /// 一天过期24小时
    /// </summary>
    public const int _1Day = 1440;

    /// <summary>
    /// 12小时过期
    /// </summary>
    public const int _12Hours = 720;

    /// <summary>
    /// 8小时过期
    /// </summary>
    public const int _8Hours = 480;

    /// <summary>
    /// 5小时过期
    /// </summary>
    public const int _5Hours = 300;

    /// <summary>
    /// 3小时过期
    /// </summary>
    public const int _3Hours = 180;

    /// <summary>
    /// 2小时过期
    /// </summary>
    public const int _2Hours = 120;

    /// <summary>
    /// 1小时过期
    /// </summary>
    public const int _1Hour = 60;

    /// <summary>
    /// 半小时过期
    /// </summary>
    public const int _30Minutes = 30;

    /// <summary>
    /// 5分钟过期
    /// </summary>
    public const int _5Minutes = 5;

    /// <summary>
    /// 1分钟过期
    /// </summary>
    public const int _1Minutes = 1;

    /// <summary>
    /// 永不过期
    /// </summary>
    public const int Never = -1;
}