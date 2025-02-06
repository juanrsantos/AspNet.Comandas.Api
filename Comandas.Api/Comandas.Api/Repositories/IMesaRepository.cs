using Comandas.Api.Dtos;

namespace Comandas.Api.Repositories
{
    public interface IMesaRepository
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);
    }
}