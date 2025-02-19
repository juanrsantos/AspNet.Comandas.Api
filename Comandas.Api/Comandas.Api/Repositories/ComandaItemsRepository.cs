using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Repositories
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
    }
}
