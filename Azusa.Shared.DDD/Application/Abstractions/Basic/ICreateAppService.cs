using System.Threading.Tasks;

namespace Azusa.Shared.DDD.Application.Abstractions.Basic;

public interface ICreateAppService<TOutputDto, in TCreateDto> :
    IApplicationService
{
    Task<TOutputDto> CreateAsync(TCreateDto input);
}