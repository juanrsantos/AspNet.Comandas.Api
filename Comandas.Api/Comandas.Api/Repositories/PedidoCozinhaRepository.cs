using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Threading;

namespace Comandas.Api.Repositories
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
