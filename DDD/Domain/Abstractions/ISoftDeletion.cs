namespace Azusa.Shared.DDD.Domain.Abstractions;

public interface ISoftDeletion
{
    public bool IsDeleted { get; set; }
}