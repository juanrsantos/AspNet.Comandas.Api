using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Threading;

namespace Comandas.Api.Repositories
{
    public class PedidoCozinhaItemsRepository : IPedidoCozinhaItemsRepository
    {
        private readonly ComandaDbContext _context;
        public PedidoCozinhaItemsRepository(ComandaDbContext comandaDbContext)
        {
            this._context = comandaDbContext;
        }

        public async Task AddAsync(PedidoCozinhaItem pedidoCozinha)
        {
            await _context.PedidoCozinhaItems.AddAsync(pedidoCozinha);
        }

        public Task AddAsync(PedidoCozinha pedidoCozinha)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            throw new NotImplementedException();
        }



        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await  _context.SaveChangesAsync(cancellationToken);
        }
    }
}
