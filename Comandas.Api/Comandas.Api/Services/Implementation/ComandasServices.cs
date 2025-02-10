using Comandas.Api.Dtos;
using Comandas.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;

namespace Comandas.Api.Services.Implementation
{
    public class ComandasServices : IComandasServices
    {
        private readonly IComandasRepository _comanadasRepository;
        public ComandasServices(IComandasRepository comandaRepository)
        {
            _comanadasRepository = comandaRepository;
        }
        public async Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _comanadasRepository.GetComandasAsync(cancellationToken, page, pageSize);
        }
    }
}
