using Azusa.Shared.DDD.Application.Abstractions.Basic;

namespace Azusa.Shared.DDD.Application.Abstractions;

public interface ICrudAppService<in TKey, TOutputDto, in TCreateUpdateInput> :
    ICrudAppService<TKey, TOutputDto, TCreateUpdateInput, TCreateUpdateInput> { }

public interface ICrudAppService<in TKey, TOutputDto, in TCreateInput, in TUpdateInput> :
    ICreateUpdateAppService<TKey, TOutputDto, TCreateInput, TUpdateInput>,
    IRetrieveAppService<TKey, TOutputDto, TOutputDto>,
    IDeleteAppService<TKey> { }