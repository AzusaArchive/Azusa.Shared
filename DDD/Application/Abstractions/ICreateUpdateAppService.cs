using Azusa.Shared.DDD.Application.Abstractions.Basic;

namespace Azusa.Shared.DDD.Application.Abstractions;

public interface ICreateUpdateAppService<in TKey, TOutputDto, in TCreateUpdateInput> :
    ICreateUpdateAppService<TKey, TOutputDto, TCreateUpdateInput, TCreateUpdateInput> 
{ }

public interface ICreateUpdateAppService<in TKey, TOutputDto, in TCreateInput, in TUpdateInput>
    : ICreateAppService<TOutputDto, TCreateInput>,
        IUpdateAppService<TOutputDto, TKey, TUpdateInput> 
{ }