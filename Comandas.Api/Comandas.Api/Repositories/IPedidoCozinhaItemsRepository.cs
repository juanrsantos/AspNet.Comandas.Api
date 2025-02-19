using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Repositories
{
    public interface IPedidoCozinhaItemsRepository
    {
        Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task AddAsync(PedidoCozinhaItem pedidoCozinhaItem);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}