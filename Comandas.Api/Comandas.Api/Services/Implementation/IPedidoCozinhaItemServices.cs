
using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Services.Implementation
{
    public interface IPedidoCozinhaItemServices
    {
        Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task AddAsync(PedidoCozinhaItem novoPedidoCozinhaItem);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

    }
}