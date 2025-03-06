using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Services
{
    public interface IComandasServices
    {
        Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<ComandaGetDTO> Get(int id);

        Task<Comanda> Post(ComandaDTO comanda);
        Task UpdateComandaAsync(ComandaUpdateDTO comanda);
    }
}