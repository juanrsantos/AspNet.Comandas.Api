using Comandas.Data.Repositories.Interfaces;
using Comandas.Shared.Dtos;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Comandas.Data.Repositories.Implementation
{
    public class RedisRepository : IRedisRepository
    {
        // 02 variaveis para Criar/Consumir o REDIS
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;


        public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> SaveAsync<T>(string key, T dados, TimeSpan? ttl) 
        {
            string serialize = JsonConvert.SerializeObject(dados);
            return await _database.StringSetAsync(key, serialize, ttl);
        }

        public async Task<T?> GetAsync<T>(string key)
        {

            var dado = await _database.StringGetAsync(key);
            if(dado.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(dado);
        }

        

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task<PagedResponseDto<T?>> GetMesasAsync<T>(string key)
        {
            var dado = await _database.StringGetAsync(key);

            if (dado.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<PagedResponseDto<T>>(dado);
        }
    }
}
