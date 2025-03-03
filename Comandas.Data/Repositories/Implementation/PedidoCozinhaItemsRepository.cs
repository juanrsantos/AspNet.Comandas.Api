using Comandas.Api.Models;
using Comandas.Data.Repositories.Interfaces;
using Comandas.Shared.Dtos;

namespace Comandas.Data.Repositories.Implementation
{
    // Construtor implicito , passando o contexto para a classe generica repository.
    public class PedidoCozinhaItemsRepository (ComandaDbContext comandaDbContext) : Repository(comandaDbContext), IPedidoCozinhaItemsRepository
    {
   
        public async Task AddAsync(PedidoCozinhaItem pedidoCozinha)
        {
            await comandaDbContext.PedidoCozinhaItems.AddAsync(pedidoCozinha);
        }

        public Task AddAsync(PedidoCozinha pedidoCozinha)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

    }
}
