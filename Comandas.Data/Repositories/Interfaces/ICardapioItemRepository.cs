using Comandas.Api.Models;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface ICardapioItemRepository
    {
        Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem);
    }
}