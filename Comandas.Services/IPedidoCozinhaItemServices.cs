using Comandas.Api.Models;
using Comandas.Shared.Dtos;

namespace Comandas.Api.Services.Implementation
{
    public interface IPedidoCozinhaItemServices
    {
        Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task AddAsync(PedidoCozinhaItem novoPedidoCozinhaItem);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

    }
}