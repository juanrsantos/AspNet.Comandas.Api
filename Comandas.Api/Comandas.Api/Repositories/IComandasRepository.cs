using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Comandas.Api.Repositories
{
    public interface IComandasRepository
    {
        Task AddAsync(Comanda novaComanda);
        Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize);
    }
}