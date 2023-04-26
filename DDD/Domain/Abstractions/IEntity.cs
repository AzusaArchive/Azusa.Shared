namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 实体接口
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IEntity<TKey> where TKey : IComparable
{
    public TKey Id { get; set; }
}