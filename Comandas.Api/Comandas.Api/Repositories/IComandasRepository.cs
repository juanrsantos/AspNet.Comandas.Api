using Comandas.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Comandas.Api.Repositories
{
    public interface IComandasRepository
    {
        Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize);
    }
}