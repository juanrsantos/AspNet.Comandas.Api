using Comandas.Api.Models;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IComandasRepository
    {
        Task AddAsync(Comanda novaComanda);
        Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<ComandaGetDTO> Get(int id);
    }
}