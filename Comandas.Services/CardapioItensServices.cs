using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;

namespace Comandas.Services
{
    public class CardapioItensServices
    {
        private readonly ICardapioItemRepository _cardapioItemRepository;

        public CardapioItensServices(ICardapioItemRepository cardapioItemRepository)
        {
            _cardapioItemRepository = cardapioItemRepository;
        }

        public async Task<CardapioItemDTO> AddCardapioItemAsync(CardapioItemDTO cardapioitem)
        {

            if (string.IsNullOrEmpty(cardapioitem.Titulo))
            {
                throw new BadRequestException("O título é obrigatorio.");
            }

            var novoCardapioItem = new CardapioItem
            {
                Titulo = cardapioitem.Titulo,
                Descricao = cardapioitem.Descricao,
                Id = cardapioitem.Id,
                Preco = cardapioitem.Preco,
                PossuiPreparo = cardapioitem.PossuiPreparo,
            };

             await _cardapioItemRepository.AddCardapioItemAsync(novoCardapioItem);

            return new CardapioItemDTO
            {

                Titulo = novoCardapioItem.Titulo,
                Descricao = novoCardapioItem.Descricao,
                Id = novoCardapioItem.Id,
                Preco = novoCardapioItem.Preco,
                PossuiPreparo = novoCardapioItem.PossuiPreparo,
            };


        }


        public async Task UpdateCardapioItemAsync(CardapioItemDTO cardapioitem)
        {

            var cardapioItemExistente = await _cardapioItemRepository.GetCardapioItemPorId(cardapioitem.Id);

            if (cardapioitem.Id != cardapioitem.Id)
            {
               throw new BadRequestException("O ID fornecido não corresponde ao ID do corpo da requisição ");
            }
            if (cardapioItemExistente == null) 
            {
                throw new NotFoundException("Cardápio item não encontrado para atualização");
            }

            if (String.IsNullOrEmpty(cardapioitem.Titulo))
            {
                throw new BadRequestException("O título é obrigatorio.");
            }

            cardapioItemExistente.Titulo = cardapioitem.Titulo;
            cardapioItemExistente.Descricao = cardapioitem.Descricao;
            cardapioItemExistente.Preco = cardapioitem.Preco;   
            cardapioItemExistente.PossuiPreparo = cardapioitem.PossuiPreparo;

            await _cardapioItemRepository.UpdateCardapioItemAsync(cardapioItemExistente);


        }




        public async Task<CardapioItemDTO> GetCardapioItem(int id)
        {
            try
            {
                var cardapioitem = await _cardapioItemRepository.GetCardapioItemPorId(id);

                if (cardapioitem is null)
                {
                    throw new NotFoundException($"Cardapio Item com {id} não encontrado");
                }

                var cardapioItemDto = new CardapioItemDTO
                {
                    Descricao = cardapioitem.Descricao,
                    Id = cardapioitem.Id,
                    Preco = cardapioitem.Preco,
                    Titulo = cardapioitem.Titulo,
                    PossuiPreparo = cardapioitem.PossuiPreparo
                };

                return cardapioItemDto;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter a comanda", ex);
            }

        }

        public async Task<PagedResponseDto<CardapioItemDTO>> GetCardapioItensAsync(int page, int pageSize, CancellationToken cancellationToken)
        {

            var pagedResponseCardapioItemDto = await _cardapioItemRepository.GetCardapioItensAsync(page, pageSize, cancellationToken);

            if (pagedResponseCardapioItemDto is null)
            {
                throw new NotFoundException($"Cardápio Itens não encontrado");
            }

            return pagedResponseCardapioItemDto;
        }

        public async Task ExcluirCardapioItemAsync(int id)
        {
            var objExcluir = await _cardapioItemRepository.GetCardapioItemPorId(id);

            if(objExcluir is null)
            {
                throw new NotFoundException("Objeto não encontrado para exclusão");
            }

            await _cardapioItemRepository.ExcluirCardapioItemAsync(objExcluir);

        }
    }
}
