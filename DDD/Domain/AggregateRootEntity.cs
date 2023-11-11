using Azusa.Shared.DDD.Domain.Abstractions;

namespace Azusa.Shared.DDD.Domain;

/// <summary>
/// 聚合根实体，带有软删除，创建时间，删除时间，修改时间属性
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class AggregateRootEntity<TKey> : IEntity<TKey>, ISoftDeletion, IHasCreationTime, IHasDeletionTime,
    IHasModificationTime where TKey : IComparable
{
    public required TKey Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationTime { get; } = DateTime.Now;
    public DateTime? DeletionTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public virtual void SoftDelete() => IsDeleted = false;
}