using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Threading;

namespace Comandas.Api.Repositories
{
    public class MesaRepository : IMesaRepository
    {
        private readonly ComandaDbContext _context;
        public MesaRepository(ComandaDbContext comandaDbContext)
        {
            this._context = comandaDbContext;
        }


        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            var query = _context.Mesas.AsQueryable();
            var count = await query.CountAsync();

            var mesas = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .TagWith("GetMesas").AsNoTracking().Select(x => new MesaDTO
                {
                    Id = x.Id,
                    NumeroMesa = x.NumeroMesa,
                    SituacaoMesa = x.SituacaoMesa
                }).ToListAsync(cancellationToken);

            var res = new PagedResponseDto<MesaDTO>(mesas, count, page, pageSize);
            return res;
        }

    }
}
