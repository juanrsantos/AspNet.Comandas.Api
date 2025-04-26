using Comandas.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IRedisRepository
    {
        Task<bool> SaveAsync<T>(string key, T dados, TimeSpan? ttl);
        Task<T?> GetAsync<T>(string key);

        Task<PagedResponseDto<T?>> GetMesasAsync<T>(string key);

        Task RemoveAsync(string key);

    }
}
