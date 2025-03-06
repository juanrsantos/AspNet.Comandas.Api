using Comandas.Domain;

namespace Comandas.Api.Services.Implementation
{
    public interface IPedidoCozinhaServices
    {
        Task AddAsync(PedidoCozinha novoPedidoCozinha);
    }
}