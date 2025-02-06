using Comandas.Api.Dtos;

namespace Comandas.Api.Services.Implementation
{
    public interface IMesaServices
    {
        Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize);
    }
}