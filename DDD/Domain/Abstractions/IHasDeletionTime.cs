using System;

namespace Azusa.Shared.DDD.Domain.Abstractions;

public interface IHasDeletionTime : ISoftDeletion
{
    DateTime? DeletionTime { get; set; }
}