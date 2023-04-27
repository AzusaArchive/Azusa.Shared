using Azusa.Shared.DDD.Application.Abstractions.Basic;

namespace Azusa.Shared.DDD.Application.Abstractions;

public interface ICrudAppService<TEntity, in TKey> :
    ICrudAppService<TEntity, TKey, TEntity> { }

public interface ICrudAppService<TEntity, in TKey, TOutputDto> :
    ICrudAppService<TEntity, TKey, TOutputDto, TEntity> { }

public interface ICrudAppService<out TEntity, in TKey, TOutputDto, in TCreateUpdateInput> :
    ICrudAppService<TEntity, TKey, TOutputDto, TCreateUpdateInput, TCreateUpdateInput> { }

public interface ICrudAppService<out TEntity, in TKey, TOutputDto, in TCreateInput, in TUpdateInput> :
    ICreateUpdateAppService<TKey, TOutputDto, TCreateInput, TUpdateInput>,
    IRetrieveAppService<TEntity, TKey, TOutputDto, TOutputDto>,
    IDeleteAppService<TKey> { }