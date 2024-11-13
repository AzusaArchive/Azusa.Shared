using System;

namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 删除时间接口，实现软删除<see cref="ISoftDeletion"/>和删除时间属性
/// </summary>
public interface IHasDeletionTime : ISoftDeletion
{
    DateTime? DeletionTime { get; set; }
}