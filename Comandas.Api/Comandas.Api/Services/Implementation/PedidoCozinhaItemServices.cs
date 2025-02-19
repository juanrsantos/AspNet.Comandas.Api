using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Comandas.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Services.Implementation
{
    public class PedidoCozinhaItemServices : IPedidoCozinhaItemServices
    {
        private readonly IPedidoCozinhaItemsRepository _repository;

        public PedidoCozinhaItemServices(IPedidoCozinhaItemsRepository pedidoCozinhaRepository)
        {
            _repository = pedidoCozinhaRepository;
        }

        public async Task AddAsync(PedidoCozinhaItem novoPedidoCozinhaItem)
        {
          await _repository.AddAsync(novoPedidoCozinhaItem);
        }

        public async Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _repository.GetPedidoCozinhaItemsAsync(cancellationToken, page, pageSize);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken? cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
