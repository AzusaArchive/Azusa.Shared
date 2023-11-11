namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 修改时间接口，带有修改时间属性
/// </summary>
public interface IHasModificationTime
{
    public DateTime? LastModificationTime { get; set; }
}