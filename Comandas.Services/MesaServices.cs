


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
        private readonly IRedisRepository _redisRepository;

        public MesaServices(IMesaRepository mesaRepository, IRedisRepository redisRepository)
        {
            _repository = mesaRepository;
            _redisRepository = redisRepository;
        }

        public async Task AddAsync(Mesa mesa)
        {
            await _repository.AddAsync(mesa);
        }

        public async Task<MesaDTO> GetMesaAsync(int id)
        {
            // Cache defensivo.
            var cacheKey = $"mesa:{id}";
            var cached = await _redisRepository.GetAsync<MesaDTO>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            var mesa = await _repository.GetMesa(id);
            if (mesa != null)
            {
                await _redisRepository.SaveAsync<MesaDTO>(cacheKey, mesa, TimeSpan.FromMinutes(1));
            }
            return mesa;
        }

        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            var cacheKey = $"mesas:{page}:{pageSize}";
            var cached = await _redisRepository.GetMesasAsync<MesaDTO?>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var mesas = await _repository.GetMesasAsync(cancellationToken, page, pageSize);

            if (mesas != null)
            {
                await _redisRepository.SaveAsync<PagedResponseDto<MesaDTO>>(cacheKey, mesas, TimeSpan.FromMinutes(1));
            }
            return mesas;
        }

        public async Task RemoveMesaAsync(int id)
        {
          
            Mesa mesa =await  _repository.GetMesaById(id);


            if (mesa is null)
            {
                throw new NotFoundException("Mesa não encontrada");
            }
            var cacheKey = $"mesa:{id}";
            
            var cached = await _redisRepository.GetAsync<MesaDTO>(cacheKey);
            if (cached != null)
            {
                await _redisRepository.RemoveAsync(cacheKey);
            }


            await _repository.RemoveMesaAsync(mesa);
        }

        public async Task SaveChangesAsync(CancellationToken? cancellationToken)
        {
             await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMesaAsync(Mesa mesa)
        {
            var cacheKey = $"mesa:{mesa.Id}";
            var cached = await _redisRepository.GetAsync<MesaDTO>(cacheKey);

            if (cached != null)
            {
                await _redisRepository.RemoveAsync(cacheKey);
            }
            await _repository.UpdateMesaAsync(mesa);
        }

    }
}
