using Comandas.Domain;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface ICardapioItemRepository
    {
        Task<CardapioItemDTO> AddCardapioItemAsync(CardapioItem cardapioitem);
        Task ExcluirCardapioItemAsync(CardapioItem objExcluir);
        Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem);
        Task<PagedResponseDto<CardapioItemDTO>> GetCardapioItensAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task UpdateCardapioItemAsync(CardapioItem cardapioItemExistente);
    }
}