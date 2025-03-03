using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Services
{
    public class MesaServices : IMesaServices
    {
        private readonly IMesaRepository _repository;

        public MesaServices(IMesaRepository mesaRepository)
        {
            _repository = mesaRepository;
        }

        public Task AddAsync(Mesa mesa)
        {
            throw new NotImplementedException();
        }

        public Task<MesaDTO> GetMesa(int id)
        {
           return _repository.GetMesa(id);
        }

        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _repository.GetMesasAsync(cancellationToken, page, pageSize);
        }

        Task<Mesa> IMesaServices.GetMesa(int id)
        {
            throw new NotImplementedException();
        }
    }
}
