using System;

namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 创建时间接口，带有创建时间属性
/// </summary>
public interface IHasCreationTime
{
    public DateTime CreationTime { get; }
}