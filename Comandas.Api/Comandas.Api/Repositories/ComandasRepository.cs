using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Repositories
{
    public class ComandasRepository : IComandasRepository
    {
        private readonly ComandaDbContext _context;

        public ComandasRepository(ComandaDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddAsync(Comanda novaComanda)
        {
            await _context.Comandas.AddAsync(novaComanda);
        }

        public async Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            var query = _context.Comandas.AsQueryable();
            var count = await query.CountAsync();

            var comandas = await query.Skip((page - 1) * pageSize).Take(pageSize).Take(pageSize)
                .TagWith("GetComandas").AsNoTracking()
                .Select(x => new ComandaGetDTO
                {
                    Id = x.Id,
                    NroMesa = x.NumeroMesa,
                    NomeCliente = x.NomeCliente,
                    comandaItems = x.ComandaItems.Select(u => new ComandaItemGetDto
                    {
                        Id = u.Id,
                        Titulo = u.CardapioItem.Titulo
                    }).ToList()
                }).ToListAsync();

            var res = new PagedResponseDto<ComandaGetDTO>(comandas, count, page, pageSize);
            return res;
        }

    }
}
