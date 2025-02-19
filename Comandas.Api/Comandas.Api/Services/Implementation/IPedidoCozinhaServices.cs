
using Comandas.Api.Dtos;
using Comandas.Api.Models;

namespace Comandas.Api.Services.Implementation
{
    public interface IPedidoCozinhaServices
    {
        Task AddAsync(PedidoCozinha novoPedidoCozinha);
    }
}