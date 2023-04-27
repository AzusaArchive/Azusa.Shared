using System.Collections.Generic;
using System.Threading.Tasks;
using Azusa.Shared.Search;

namespace Azusa.Shared.DDD.Application.Abstractions.Basic;

public interface IRetrieveAppService<in TKey, TOutputDto, TGetListOutputDto> :
    IApplicationService
{
    Task<TOutputDto?> FindAsync(TKey id);
    Task<TOutputDto> GetAsync(TKey id);
    Task<List<TGetListOutputDto>> GetListAsync(SearchRule? rule = null);
}