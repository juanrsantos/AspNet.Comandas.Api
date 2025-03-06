using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;

namespace Comandas.Api.Services.Implementation
{
    public class PedidoCozinhaServices : IPedidoCozinhaServices
    {
        private readonly IPedidoCozinhaRepository _repository;

        public PedidoCozinhaServices(IPedidoCozinhaRepository pedidoCozinhaRepository)
        {
            _repository = pedidoCozinhaRepository;
        }

        public async Task AddAsync(PedidoCozinha novoPedidoCozinha)
        {
            _repository.AddAsync(novoPedidoCozinha);
        }

  
    }
}
