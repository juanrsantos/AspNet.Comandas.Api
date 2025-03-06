using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Data.Repositories.Implementation
{
    public class ComandasRepository(ComandaDbContext _context) : Repository(_context), IComandasRepository
    {
        private readonly ComandaDbContext _context = _context;

        public async Task AddAsync(Comanda novaComanda)
        {
            await _context.Comandas.AddAsync(novaComanda);
        }

        public async Task<Comanda?> GetById(int id)
        {
            try
            {
                var query = _context.Comandas.AsQueryable();
                var comanda = await query.TagWith("GetPorId")
                    .Include(x => x.ComandaItems)
                  .FirstOrDefaultAsync(x => x.Id == id);

                return comanda;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar", ex);
            }
        }

        public async Task<ComandaGetDTO?> Get(int id)
        {
            try
            {
                var query = _context.Comandas.AsQueryable();
                var comanda = await query.TagWith("GetPorId")
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
                     }).FirstOrDefaultAsync(x => x.Id == id);

                return comanda;
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro ao consultar", ex);
            }
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

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<bool> ComandaExiste(int id)
        {
            return await _context.Comandas.AsNoTracking().AnyAsync(x => x.Id == id);
        }
    }
}
