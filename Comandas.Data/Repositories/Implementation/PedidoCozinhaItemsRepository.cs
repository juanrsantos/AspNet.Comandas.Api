using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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

        public async Task<PagedResponseDto<PedidoCozinhaGetDto>> GetPedidoCozinhaItemsAsync(CancellationToken cancellationToken, int page, int pageSize, int? situacao)
        {
            // Incluir todas as tabelas na consulta
            var query = comandaDbContext.PedidoCozinhas.AsNoTracking()
                        .Include(x => x.Comanda)
                        .Include(x => x.PedidoCozinhaItems)
                            .ThenInclude(x => x.ComandaItem)
                                .ThenInclude(x => x.CardapioItem)
                                .AsQueryable();

            var count = await query.CountAsync();


            var pedidoCozinhas = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .TagWith("GetPedidos").AsNoTracking().Select(x => new PedidoCozinhaGetDto
                {
                    Id = x.Id,
                    NomeCliente = x.Comanda.NomeCliente,
                    NumeroMesa = x.Comanda.NumeroMesa,
                    Titulo = x.PedidoCozinhaItems.First().ComandaItem.CardapioItem.Titulo,
                }).ToListAsync(cancellationToken);


            // Adicionar o filtro where se a situação informada
            if (situacao > 0)
            {
                query = query.Where(x => x.SituacaoId == situacao);
            }


            return new PagedResponseDto<PedidoCozinhaGetDto>(pedidoCozinhas, count, page, pageSize);
            // Executar a consulta do banco e retornar o DTO
        }

    }
}
