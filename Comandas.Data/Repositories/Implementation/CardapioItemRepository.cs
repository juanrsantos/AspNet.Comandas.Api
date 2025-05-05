using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Comandas.Data.Repositories.Implementation
{
    public class CardapioItemRepository : ICardapioItemRepository
    {
        private readonly ComandaDbContext _context;

        public CardapioItemRepository(ComandaDbContext _context)
        {
            this._context = _context;
        }


        public async Task AddCardapioItemAsync(CardapioItem cardapioitem)
        {
            try
            {
                await _context.CardapioItems.AddAsync(cardapioitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Erro ao salvar o CardapioItem no banco de dados",ex);
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro inesperado no repositorio",ex);
            }
        }

        public async Task ExcluirCardapioItemAsync(CardapioItem objExcluir)
        {
            try
            {
                _context.CardapioItems.Remove(objExcluir);
                await _context.SaveChangesAsync();
            }
            catch(DbException ex)
            {
                throw new RepositoryException("Erro ao excluir CardapioItem no banco.", ex);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("O registro foi alterado ou já excluído por outro usuário.", ex);
            }
            catch(Exception ex)
            {
                throw new RepositoryException("Erro inesperado ao excluir CardapioItem.", ex);
            }
        }

        public Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem)
        {

            return  _context.CardapioItems.FirstOrDefaultAsync(x => x.Id == numeroCardapioItem);
        }

        public async Task<PagedResponseDto<CardapioItemDTO>> GetCardapioItensAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.CardapioItems.AsQueryable();
                var count = await query.CountAsync();

                var cardapioItens = await query.Skip((page - 1) * pageSize).Take(pageSize)
                    .TagWith("GetCardapioItens").AsNoTracking().Select(x => new CardapioItemDTO
                        {
                            Id = x.Id,
                            Descricao = x.Descricao,
                            Preco = x.Preco,
                            PossuiPreparo = x.PossuiPreparo,
                            Titulo = x.Titulo,
                        }).ToListAsync();


                return new PagedResponseDto<CardapioItemDTO>(cardapioItens, count, page, pageSize);
            }
            catch(Exception ex)
            {
                throw new RepositoryException("Erro ao buscar cardapio itens. Por favor, tente novamente mais tarde. ", ex);
            }
        }

        public async Task UpdateCardapioItemAsync(CardapioItem cardapioItemExistente)
        {
            try
            {
                 _context.Update(cardapioItemExistente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("O registro foi alterado por outro usuário");
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Erro ao atualizar CardapioItem no banco.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Erro inesperad ao atualizar", ex);
            }
        }

        Task<CardapioItemDTO> ICardapioItemRepository.AddCardapioItemAsync(CardapioItem cardapioitem)
        {
            throw new NotImplementedException();
        }
    }
}
