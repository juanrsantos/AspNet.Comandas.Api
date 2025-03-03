using Comandas.Api.Models;
using Comandas.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Data.Repositories.Implementation
{
    public class CardapioItemRepository : ICardapioItemRepository
    {
        private readonly ComandaDbContext _context;

        public CardapioItemRepository(ComandaDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddAsync(CardapioItem novoCardapioItem)
        {
            await _context.CardapioItems.AddAsync(novoCardapioItem);
        }

        public Task<CardapioItem?> GetCardapioItemPorId(int numeroCardapioItem)
        {
            return  _context.CardapioItems.FirstOrDefaultAsync(x => x.Id == numeroCardapioItem);
        }
    }
}
