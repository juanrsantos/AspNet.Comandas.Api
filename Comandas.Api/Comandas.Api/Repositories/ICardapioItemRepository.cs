using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Repositories
{
    public interface ICardapioItemRepository
    {
        Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem);
    }
}