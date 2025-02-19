using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Comandas.Api.Repositories
{
    public interface IComandaItemsRepository
    {
        Task AddAsync(ComandaItem novaComandaItem);

        Task<IEnumerable<ComandaItemGetDto>> GetItensdaComanda(int id);
    }
}