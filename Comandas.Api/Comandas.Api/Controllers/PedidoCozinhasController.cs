using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

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


        [SwaggerOperation(Summary = "Obtém todas os pedidos da cozinha", Description = "Lista todos os pedidos da cozinha")]
        [SwaggerResponse(200, "retorna a lista de pedidos", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoCozinhaGetDto>>> GetPedidosAsync([FromQuery] int? situacao, CancellationToken cancelattiontoken)
        {
            // Incluir todas as tabelas na consulta
            var query = _context.PedidoCozinhas.AsNoTracking()
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

        [SwaggerOperation(Summary = "Edita o pedido da cozinha, atualizando seu status", Description = "Edita o pedido da cozinha, atualizando seu status")]
        [SwaggerResponse(204, "Edita o pedido da cozinha", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Pedido não encontrado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidoCozinha(int id, [FromBody] int novoStatusId)
        {
            var pedidoCozinha = await _context.PedidoCozinhas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
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
