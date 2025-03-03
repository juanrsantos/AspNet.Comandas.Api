using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Services
{
    public interface IMesaServices
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<Mesa> GetMesa(int id);

        Task AddAsync(Mesa mesa);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

    }
}