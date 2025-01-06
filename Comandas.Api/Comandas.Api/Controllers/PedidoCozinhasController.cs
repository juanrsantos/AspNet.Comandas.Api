using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Controllers
{
    [Tags("03. Pedido Cozinha")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoCozinhasController : ControllerBase
    {
        private readonly ComandaDbContext _context;

        public PedidoCozinhasController(ComandaDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoCozinhaGetDto>>> GetPedidosAsync([FromQuery] int? situacao, CancellationToken cancelattiontoken)
        {
            // Incluir todas as tabelas na consulta
            var query = _context.PedidoCozinhas
                        .Include(x => x.Comanda)
                        .Include(x => x.PedidoCozinhaItems)
                            .ThenInclude(x => x.ComandaItem)
                                .ThenInclude(x => x.CardapioItem)
                                .AsQueryable();
            // Adicionar o filtro where se a situação informada
            if (situacao > 0)
            {
                query = query.Where(x => x.SituacaoId == situacao);
            }
            // Executar a consulta do banco e retornar o DTO
            return Ok(await query.Select(x => new PedidoCozinhaGetDto
            {
                Id = x.Id,
                NomeCliente = x.Comanda.NomeCliente,
                NumeroMesa = x.Comanda.NumeroMesa,
                Titulo = x.PedidoCozinhaItems.First().ComandaItem.CardapioItem.Titulo,
            }).ToListAsync(cancelattiontoken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidoCozinha(int id, [FromBody] int novoStatusId)
        {
            var pedidoCozinha = await _context.PedidoCozinhas.FirstOrDefaultAsync(x => x.Id == id);
            
            if(pedidoCozinha is null)
            {
                return NotFound($"Pedido {id} não encontrado");
            }

            pedidoCozinha.SituacaoId = novoStatusId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
