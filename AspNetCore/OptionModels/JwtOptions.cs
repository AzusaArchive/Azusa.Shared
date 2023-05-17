namespace Azusa.Shared.AspNetCore.OptionModels;

/// <summary>
/// jwt报文配置数据模型，从配置系统中读取
/// </summary>
public class JwtOption
{
    /// <summary>
    /// JWT密钥
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// 过期时间，以秒为单位
    /// </summary>
    public uint ExpireSeconds { get; set; }
}