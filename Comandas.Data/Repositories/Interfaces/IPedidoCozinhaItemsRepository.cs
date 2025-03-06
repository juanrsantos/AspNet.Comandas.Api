using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IPedidoCozinhaItemsRepository
    {
        Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task AddAsync(PedidoCozinhaItem pedidoCozinhaItem);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}