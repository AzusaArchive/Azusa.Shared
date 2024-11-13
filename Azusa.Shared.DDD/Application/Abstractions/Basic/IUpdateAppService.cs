using System.Threading.Tasks;

namespace Azusa.Shared.DDD.Application.Abstractions.Basic;

public interface IUpdateAppService<TOutputDto, in TKey, in TUpdateInput> :
    IApplicationService
{
    Task<TOutputDto> UpdateAsync(TKey id, TUpdateInput input);
}