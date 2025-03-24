using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Services.Interfaces
{
    public interface IMesaServices
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<MesaDTO> GetMesaAsync(int id);

        Task AddAsync(Mesa mesa);

        Task UpdateMesaAsync(Mesa mesa);

        Task RemoveMesaAsync(int id);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

    }
}