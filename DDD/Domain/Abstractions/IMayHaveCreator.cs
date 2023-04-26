namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 实现此接口的类必须具有创建者Id属性，可以为空
/// </summary>
/// <typeparam name="TCreatorKey"></typeparam>
public interface IMayHaveCreator<TCreatorKey>
{
    TCreatorKey? CreatorId { get; }
}

/// <summary>
/// 实现此接口的类必须具有创建者Id以及创建者属性，可以为空
/// </summary>
/// <typeparam name="TCreator"></typeparam>
/// <typeparam name="TCreatorKey"></typeparam>
public interface IMayHaveCreator<out TCreator, TCreatorKey> : IMayHaveCreator<TCreatorKey> where TCreatorKey : IComparable
{
    TCreator? Creator { get; }
}