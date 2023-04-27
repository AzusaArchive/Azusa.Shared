using System.Threading.Tasks;

namespace Azusa.Shared.DDD.Application.Abstractions.Basic;

public interface IDeleteAppService<in TKey> :
    IApplicationService
{
    Task DeleteAsync(TKey id);
}