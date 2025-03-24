using Comandas.Domain;

namespace Comandas.Services.Interfaces
{
    public interface IPedidoCozinhaServices
    {
        Task AddAsync(PedidoCozinha novoPedidoCozinha);
    }
}