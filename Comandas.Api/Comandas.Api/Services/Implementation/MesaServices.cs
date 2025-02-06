using Comandas.Api.Dtos;
using Comandas.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Services.Implementation
{
    public class MesaServices : IMesaServices
    {
        private readonly IMesaRepository _repository;

        public MesaServices(IMesaRepository mesaRepository)
        {
            _repository = mesaRepository;
        }

        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _repository.GetMesasAsync(cancellationToken, page, pageSize);
        }
    }
}
