using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azusa.Shared.DDD.Application.Abstractions;
using Azusa.Shared.Exception;
using Azusa.Shared.Search;
using Microsoft.EntityFrameworkCore;

// ReSharper disable PossibleMultipleEnumeration

namespace Azusa.Shared.DDD.EntityFramework;

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
                query = query.Where(SearchRuleHelper.BuildKeywordSearchExpression<TEntity>(rule.Keyword));
            if (rule.Sorting is not null)
                if (rule.Descending)
                    query = query.OrderByDescending(SearchRuleHelper.BuildSortingExpression<TEntity>(rule.Sorting));
                else
                    query = query.OrderBy(SearchRuleHelper.BuildSortingExpression<TEntity>(rule.Sorting));
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
    public EFCoreCrudAppService(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
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
    public EFCoreCrudAppService(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
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
        var entity = await base.CreateAsync(ThrowIfMapFailed<TCreateInput, TEntity>(Mapper.Map<TEntity>(input)));
        return ThrowIfMapFailed<TEntity, TOutputDto>(Mapper.Map<TOutputDto>(entity));
    }

    public virtual async Task<TOutputDto> UpdateAsync(TKey id, TUpdateInput input)
    {
        var entity = await base.UpdateAsync(id, ThrowIfMapFailed<TUpdateInput, TEntity>(Mapper.Map<TEntity>(input)));
        return ThrowIfMapFailed<TEntity, TOutputDto>(Mapper.Map<TOutputDto>(entity));
    }

    public new virtual async Task<TOutputDto?> FindAsync(TKey id)
    {
        return Mapper.Map<TOutputDto>(await base.FindAsync(id));
    }

    public new async Task<TOutputDto> GetAsync(TKey id)
    {
        var entity = await base.GetAsync(id);
        return ThrowIfMapFailed<TEntity, TOutputDto>(Mapper.Map<TOutputDto>(entity));
    }

    public new virtual Task<List<TOutputDto>> GetListAsync(SearchRule? rule = null)
    {
        var query = GetQueryableList(rule);
        return query.ProjectTo<TOutputDto>(Mapper.ConfigurationProvider).ToListAsync();
    }

    protected TOut ThrowIfMapFailed<TIn, TOut>(TOut? entity)
    {
        if (entity is null)
            throw new ServerErrorException($"不支持的类型映射：{typeof(TIn).Name} -> {typeof(TOut).Name}");
        return entity;
    }
}