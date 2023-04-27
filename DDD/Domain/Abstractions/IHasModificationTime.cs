using System;

namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 实现此接口的类具有修改时间属性，可空
/// </summary>
public interface IHasModificationTime
{
    public DateTime? LastModificationTime { get; set; }
}