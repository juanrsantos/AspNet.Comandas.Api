using Comandas.Domain;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface ICardapioItemRepository
    {
        Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem);
    }
}