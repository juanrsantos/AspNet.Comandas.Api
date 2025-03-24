using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Services.Interfaces
{
    public interface IPedidoCozinhaItemServices
    {
        Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize, int? situacao);

        Task AddAsync(PedidoCozinhaItem novoPedidoCozinhaItem);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

    }
}