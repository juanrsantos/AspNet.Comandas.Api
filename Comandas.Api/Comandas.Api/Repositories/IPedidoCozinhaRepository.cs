using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Repositories
{
    public interface IPedidoCozinhaRepository
    {

        Task AddAsync(PedidoCozinha pedidoCozinha);
    }
}