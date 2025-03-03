using Comandas.Api.Models;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IPedidoCozinhaRepository
    {

        Task AddAsync(PedidoCozinha pedidoCozinha);
    }
}