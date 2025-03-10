using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<Mesa?> GetMesaPorNumeroMesa(int numeroMesa);

        Task<MesaDTO> GetMesa(int id);

        Task UpdateMesaAsync(Mesa mesa);

        Task SaveChangesAsync(CancellationToken? cancellationToken);

        Task RemoveMesaAsync(Mesa mesa);
        Task<Mesa> GetMesaById(int id);
        Task AddAsync(Mesa mesa);
    }
}