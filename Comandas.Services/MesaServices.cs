using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;

namespace Comandas.Services
{
    public class MesaServices : IMesaServices
    {
        private readonly IMesaRepository _repository;

        public MesaServices(IMesaRepository mesaRepository)
        {
            _repository = mesaRepository;
        }

        public async Task AddAsync(Mesa mesa)
        {
            await _repository.AddAsync(mesa);
        }

        public async Task<MesaDTO> GetMesaAsync(int id)
        {
           return await _repository.GetMesa(id);
        }

        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _repository.GetMesasAsync(cancellationToken, page, pageSize);
        }

        public async Task RemoveMesaAsync(int id)
        {
          
            Mesa mesa =await  _repository.GetMesaById(id);

            if (mesa is null)
            {
                throw new NotFoundException("Mesa não encontrada");
            }

           await _repository.RemoveMesaAsync(mesa);
        }

        public async Task SaveChangesAsync(CancellationToken? cancellationToken)
        {
             await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMesaAsync(Mesa mesa)
        {
            await _repository.UpdateMesaAsync(mesa);
        }

    }
}
