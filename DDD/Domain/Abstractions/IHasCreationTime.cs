namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 实现此接口的类必须具有创建时间属性
/// </summary>
public interface IHasCreationTime
{
    public DateTime CreationTime { get; }
}