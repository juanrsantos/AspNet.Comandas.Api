using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Data.Repositories.Implementation
{
    public class ComandaItemsRepository : IComandaItemsRepository
    {
        private readonly ComandaDbContext _context;

        public ComandaItemsRepository(ComandaDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddAsync(ComandaItem novaComandaItem)
        {
            await _context.ComandaItems.AddAsync(novaComandaItem);
        }

        public async Task<ComandaItem> GetComandaItemById(int id)
        {
           return await _context.ComandaItems.AsNoTracking().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ComandaItemGetDto>> GetItensdaComanda(int id)
        {
            var comandaItemsDto = await _context.ComandaItems.Include(x => x.CardapioItem)
                    .Where(x => x.ComandaId == id)
                    .Select(x => new ComandaItemGetDto
                    {
                        Id = x.Id,
                        Titulo = x.CardapioItem.Titulo
                    }).ToListAsync(); 

            return comandaItemsDto;
        }

        public void RemoverComandaItemAsync(ComandaItem comandaItenExcluir)
        {

            _context.Remove(comandaItenExcluir);
        }
    }
}
