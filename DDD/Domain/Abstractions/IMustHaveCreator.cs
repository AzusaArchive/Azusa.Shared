using System;

namespace Azusa.Shared.DDD.Domain.Abstractions;

/// <summary>
/// 实现此接口的实体必须具有创建者Id属性
/// </summary>
/// <typeparam name="TCreatorKey"></typeparam>
public interface IMustHaveCreator<TCreatorKey> where TCreatorKey : IComparable
{
    TCreatorKey CreatorId { get; }
}

/// <summary>
/// 实现此接口的实体必须具有创建者Id以及创建者属性
/// </summary>
/// <typeparam name="TCreator"></typeparam>
/// <typeparam name="TCreatorKey"></typeparam>
public interface IMustHaveCreator<out TCreator, TCreatorKey> : IMustHaveCreator<TCreatorKey> where TCreatorKey : IComparable
{
    TCreator Creator { get; }
}