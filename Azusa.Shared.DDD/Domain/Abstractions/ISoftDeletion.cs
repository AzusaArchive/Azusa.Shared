namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 软删除接口，带有“已删除”属性和软删除方法
/// </summary>
public interface ISoftDeletion
{
    public bool IsDeleted { get; set; }
    public void SoftDelete() => IsDeleted = true;
}