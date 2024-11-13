using System.Linq.Expressions;
using Azusa.Shared.DDD.Domain.Abstractions;
using Azusa.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Azusa.Shared.DDD.EntityFramework;

public static class EFCoreExtensions
{
    /// <summary>
    /// 开启DbContext软删除全局过滤器，实体必须实现<see cref="ISoftDeletion"/>接口才能生效
    /// </summary>
    public static void EnableSoftDeletionGlobalFilter(this ModelBuilder modelBuilder)
    {
        var typesHasSoftDeletion = modelBuilder.Model.GetEntityTypes()
            .Where(type => type.ClrType.IsAssignableTo(typeof(ISoftDeletion)));

        typesHasSoftDeletion.Foreach(type =>
        {
            //TODO:单元测试
            // 构造表达式树 entity => !entity.IsDeleted
            var param1 = Expression.Variable(typeof(ISoftDeletion), "entity");
            var propExpr = Expression.Property(param1,
                typeof(ISoftDeletion).GetProperty(nameof(ISoftDeletion.IsDeleted))!);
            var notExpr = Expression.Not(propExpr);
            type.SetQueryFilter(Expression.Lambda(notExpr, param1));
        });
    }
}