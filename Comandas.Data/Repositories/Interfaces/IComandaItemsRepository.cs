using Comandas.Api.Models;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IComandaItemsRepository
    {
        Task AddAsync(ComandaItem novaComandaItem);

        Task<IEnumerable<ComandaItemGetDto>> GetItensdaComanda(int id);
    }
}