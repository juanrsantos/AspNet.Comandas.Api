using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Repositories
{
    public interface IMesaRepository
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<Mesa?> GetMesaPorNumeroMesa(int numeroMesa);
    }
}