using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Comandas.Api.Repositories;
using Microsoft.EntityFrameworkCore;

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
