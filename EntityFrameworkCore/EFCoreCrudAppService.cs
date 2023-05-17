using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azusa.Shared.DDD.Application.Abstractions;
using Azusa.Shared.Exception;
using Azusa.Shared.Search;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

// ReSharper disable PossibleMultipleEnumeration

namespace Azusa.Shared.EntityFrameworkCore;

/// <summary>
/// 通用的CRUD应用服务，实现了基本增删改查方法，搭配ICrudAppService接口使用
/// </summary>
/// <typeparam name="TDbContext">使用的DbContext类型</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
public class EFCoreCrudAppService<TDbContext, TEntity, TKey> :
    ICrudAppService<TEntity, TKey, TEntity, TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    protected TDbContext DbContext { get; init; }

    public EFCoreCrudAppService(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity input)
    {
        var entry = await DbContext.Set<TEntity>().AddAsync(input);
        await DbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TKey id, TEntity input)
    {
        var entry = DbContext.Set<TEntity>().Update(input);
        await DbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual async Task<TEntity?> FindAsync(TKey id)
    {
        return await DbContext.FindAsync<TEntity>(id);
    }

    public async Task<TEntity> GetAsync(TKey id)
    {
        return await FindAsync(id) ?? throw new EntityNotFoundException(typeof(TEntity));
    }

    /// <summary>
    /// 根据查询条件过滤并返回IQueryable
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    protected IQueryable<TEntity> GetQueryableList(SearchRule? rule = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();
        if (rule is not null)
        {
            if (!string.IsNullOrWhiteSpace(rule.Keyword))
                query = query.Where(BuildKeywordSearchExpression(rule.Keyword));
            if (rule.Sorting is not null)
                if (rule.Descending)
                    query = query.OrderByDescending(BuildSortingExpression(rule.Sorting));
                else
                    query = query.OrderBy(BuildSortingExpression(rule.Sorting));
            if (rule.Skip is not null)
                query = query.Skip(rule.Skip.Value);
            if (rule.Take is not null)
                query = query.Take(rule.Take.Value);
        }

        return query;
    }

    public virtual Task<List<TEntity>> GetListAsync(SearchRule? rule = null)
    {
        return GetQueryableList(rule).ToListAsync();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return DbContext.Set<TEntity>().AsQueryable();
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await DbContext.FindAsync<TEntity>(id);
        if (entity == null)
            throw new ServerErrorException("无法找到对应的实体");
        DbContext.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 推断实体属性的特性和名称，并根据搜索关键字构建过滤器表达式树
    /// 注意，反射推断比较消耗性能
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    private static Expression<Func<TEntity, bool>> BuildKeywordSearchExpression(string keyword)
    {
        //获取实体类所有公用|实例属性
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
            throw new ServerErrorException("过滤属性必须是字符串");

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
            typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!, keywordExpr);
        for (int i = 1; i < propExprs.Length; i++)
        {
            var containsExpr = Expression.Call(propExprs[i],
                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!, keywordExpr);
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
    private static Expression<Func<TEntity, object>> BuildSortingExpression(string sorting)
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

/// <summary>
/// <inheritdoc cref="EFCoreCrudAppService{TDbContext, TEntity, TKey, TOutputDto, TEntity}"/>
/// </summary>
/// <typeparam name="TDbContext">使用的DbContext类型</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
/// <typeparam name="TOutputDto">输出数据传输对象类型</typeparam>
public class EFCoreCrudAppService<TDbContext, TEntity, TKey, TOutputDto> :
    EFCoreCrudAppService<TDbContext, TEntity, TKey, TOutputDto, TEntity>,
    ICrudAppService<TEntity, TKey, TOutputDto, TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    public EFCoreCrudAppService(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
}

/// <summary>
/// <inheritdoc cref="EFCoreCrudAppService{TDbContext, TEntity, TOutputDto, TKey, TCreateUpdateInput,TCreateUpdateInput}"/>
/// </summary>
/// <typeparam name="TDbContext">使用的DbContext类型</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TOutputDto">输出数据传输对象类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
/// <typeparam name="TCreateUpdateInput">创建以及修改并用的数据传输对象类型</typeparam>
public class EFCoreCrudAppService<TDbContext, TEntity, TKey, TOutputDto, TCreateUpdateInput> :
    EFCoreCrudAppService<TDbContext, TEntity, TKey, TOutputDto, TCreateUpdateInput, TCreateUpdateInput>,
    ICrudAppService<TEntity, TKey, TOutputDto, TCreateUpdateInput>
    where TDbContext : DbContext
    where TEntity : class
{
    public EFCoreCrudAppService(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
}

/// <summary>
/// <para><inheritdoc cref="EFCoreCrudAppService{TDbContext,TEntity,TKey}"/></para>
/// <para>通过注入的AutoMapper映射器自动映射DTO</para>
/// <para>TODO:将映射器抽象为接口，与AutoMapper解耦合</para>
/// </summary>
/// <typeparam name="TDbContext">使用的DbContext类型</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
/// <typeparam name="TOutputDto">输出数据传输对象类型</typeparam>
/// <typeparam name="TCreateInput">创建数据传输对象类型</typeparam>
/// <typeparam name="TUpdateInput">修改数据传输对象类型</typeparam>
public class EFCoreCrudAppService<TDbContext, TEntity, TKey, TOutputDto, TCreateInput, TUpdateInput> :
    EFCoreCrudAppService<TDbContext, TEntity, TKey>,
    ICrudAppService<TEntity, TKey, TOutputDto, TCreateInput, TUpdateInput>
    where TDbContext : DbContext
    where TEntity : class
{
    //TODO:添加自动数据校验
    protected IMapper Mapper { get; init; }

    public EFCoreCrudAppService(TDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        Mapper = mapper;
    }

    public virtual async Task<TOutputDto> CreateAsync(TCreateInput input)
    {
        
        var entity = await base.CreateAsync(Mapper.Map<TEntity>(input));
        return Mapper.Map<TOutputDto>(entity);
    }

    public virtual async Task<TOutputDto> UpdateAsync(TKey id, TUpdateInput input)
    {
        var entity = await base.UpdateAsync(id, Mapper.Map<TEntity>(input));
        return Mapper.Map<TOutputDto>(entity);
    }

    public new virtual async Task<TOutputDto?> FindAsync(TKey id)
    {
        return Mapper.Map<TOutputDto>(await base.FindAsync(id));
    }

    public new async Task<TOutputDto> GetAsync(TKey id)
    {
        var entity = await base.GetAsync(id);
        return Mapper.Map<TOutputDto>(entity);
    }

    public new virtual Task<List<TOutputDto>> GetListAsync(SearchRule? rule = null)
    {
        var query = base.GetQueryableList(rule);
        return query.ProjectTo<TOutputDto>(Mapper.ConfigurationProvider).ToListAsync();
    }
}