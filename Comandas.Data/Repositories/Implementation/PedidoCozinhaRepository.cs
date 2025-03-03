using Comandas.Api.Models;
using Comandas.Data.Repositories.Interfaces;

namespace Comandas.Data.Repositories.Implementation
{
    public class PedidoCozinhaRepository : IPedidoCozinhaRepository
    {
        private readonly ComandaDbContext _context;
        public PedidoCozinhaRepository(ComandaDbContext comandaDbContext)
        {
            this._context = comandaDbContext;
        }

        public async Task AddAsync(PedidoCozinha pedidoCozinha)
        {
            await _context.PedidoCozinhas.AddAsync(pedidoCozinha);
        }
    }
}
