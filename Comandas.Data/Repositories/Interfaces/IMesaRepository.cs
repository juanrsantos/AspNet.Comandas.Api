using Comandas.Api.Models;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<Mesa?> GetMesaPorNumeroMesa(int numeroMesa);

        Task<MesaDTO> GetMesa(int id);
    }
}