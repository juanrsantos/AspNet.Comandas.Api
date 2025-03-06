using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IComandaItemsRepository
    {
        Task AddAsync(ComandaItem novaComandaItem);
        Task<ComandaItem> GetComandaItemById(int id);
        Task<IEnumerable<ComandaItemGetDto>> GetItensdaComanda(int id);
        void RemoverComandaItemAsync(ComandaItem comandaItenExcluir);
    }
}