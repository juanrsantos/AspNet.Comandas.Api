using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Comandas.Api.Services.Implementation
{
    public interface IComandasServices
    {
        Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize);

        Task<Comanda> Post(ComandaDTO comanda);
    }
}