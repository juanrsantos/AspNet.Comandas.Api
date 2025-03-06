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

        public async Task RemoveMesaAsync(Mesa mesa)
        {
             _repository.RemoveMesaAsync(mesa);
        }

        public async Task SaveChangesAsync(CancellationToken? cancellationToken)
        {
             await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMesaAsync(Mesa mesa)
        {

            await _repository.UpdateMesaAsync(mesa);
        }

        Task<Mesa> IMesaServices.GetMesa(int id)
        {
            throw new NotImplementedException();
        }
    }
}
