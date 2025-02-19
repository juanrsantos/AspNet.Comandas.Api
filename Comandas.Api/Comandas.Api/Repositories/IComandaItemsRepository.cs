using Comandas.Api.Models;

namespace Comandas.Api.Repositories
{
    public interface IComandaItemsRepository
    {
        Task AddAsync(ComandaItem novaComandaItem);
    }
}