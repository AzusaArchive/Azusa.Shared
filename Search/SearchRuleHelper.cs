using System.Linq.Expressions;
using System.Reflection;
using Azusa.Shared.Exception;

namespace Azusa.Shared.Search;

public static class SearchRuleHelper
{
    /// <summary>
    /// 推断实体属性的特性和名称，并根据搜索关键字构建过滤器表达式树
    /// 注意，反射推断比较消耗性能
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BuildKeywordSearchExpression<TEntity>(string keyword,
        bool ignoreCase = false)
    {
        //获取实体类所有公用&实例属性
        var propInfos =
            typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
        //获取所有具有SearchKeywordAttribute特性的属性，作为目标属性
        var targetProps = propInfos.Where(info => info.GetCustomAttribute<SearchKeywordAttribute>() is not null);
        //如果没有任何属性带SearchKeywordAttribute则按照属性名匹配，如果符合"name"或是"title"或是"content"，那就作为目标属性
        if (!targetProps.Any())
            targetProps = propInfos.Where(info => info.Name.ToUpper() is "NAME" or "TITLE" or "CONTENT");
        //没有任何属性能够匹配则异常
        if (!targetProps.Any())
            throw new ServerErrorException("没有任何能够进行过滤查询的属性，请在属性上添加[SearchKeywordAttribute]启用过滤");
        //检查属性是否为字符串类型
        if (targetProps.Any(info => info.PropertyType != typeof(string)))
            throw new ServerErrorException("使用[SearchKeywordAttribute]进行过滤的属性必须是字符串");

        //构建 e => (e.Name || Title || Content || [property with attribute]).Contain(keyword) 表达式树
        //参数e表达式
        var entityExpr = Expression.Parameter(typeof(TEntity), "entity");
        //常量关键词表达式
        var keywordExpr = Expression.Constant(keyword);
        //所有目标属性的表达式
        var propExprs = targetProps.Select(info => Expression.Property(entityExpr, info)).ToArray();
        //所有的属性调用.Contain(keyword)并且进行或运算
        var firstProp = propExprs.First();
        Expression resultExpr = Expression.Call(firstProp,
            typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string),typeof(StringComparison) })!, 
            keywordExpr,
            Expression.Constant(ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        for (int i = 1; i < propExprs.Length; i++)
        {
            var containsExpr = Expression.Call(propExprs[i],
                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string),typeof(StringComparison) })!, 
                keywordExpr,
                Expression.Constant(ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
            resultExpr = Expression.Or(resultExpr, containsExpr);
        }

        //构造成委托
        var lambda = Expression.Lambda<Func<TEntity, bool>>(resultExpr, entityExpr);
        // Console.WriteLine(lambda.ToString());
        return lambda;
    }

    /// <summary>
    /// 推断实体属性的名称，根据属性名列表构建排序表达式树
    /// </summary>
    /// <param name="sorting"></param>
    /// <returns></returns>
    public static Expression<Func<TEntity, object>> BuildSortingExpression<TEntity>(string sorting)
    {
        var propInfos =
            typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
        var targetProp = propInfos.FirstOrDefault(info =>
            string.Equals(info.Name, sorting, StringComparison.CurrentCultureIgnoreCase));
        if (targetProp is null)
            throw new ServerErrorException("该实体没有与输入匹配的排序属性");

        //构建entity => entity.(targetProperty) 表达式树
        var entityExpr = Expression.Parameter(typeof(TEntity), "entity");
        var propertyExpr = Expression.Property(entityExpr, targetProp);
        var objectPropExpr = Expression.TypeAs(propertyExpr, typeof(object));
        return Expression.Lambda<Func<TEntity, object>>(objectPropExpr, entityExpr);
    }
}