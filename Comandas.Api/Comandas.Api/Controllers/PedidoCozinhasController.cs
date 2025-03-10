using Comandas.Api.Services.Implementation;
using Comandas.Data;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<PedidoCozinhasController> _logger;
        private readonly IPedidoCozinhaItemServices _pedidoCozinhaServices;

        public PedidoCozinhasController(ComandaDbContext contexto, ILogger<PedidoCozinhasController> logger, IPedidoCozinhaItemServices pedidoCozinhaServices)
        {
            _context = contexto;
            _logger = logger;
            _pedidoCozinhaServices = pedidoCozinhaServices;
        }


        [SwaggerOperation(Summary = "Obtém todas os pedidos da cozinha", Description = "Lista todos os pedidos da cozinha")]
        [SwaggerResponse(200, "retorna a lista de pedidos", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<PedidoCozinhaGetDto>>> GetPedidosAsync([FromQuery] int? situacao, CancellationToken cancelattiontoken, int page, int pageSize)
        {
            _logger.LogInformation($"[{nameof(GetPedidosAsync)}] Iniciando consulta de pedidos");
            try
            {
                var pedidosCozinha = _pedidoCozinhaServices.GetPedidoCozinhaItemsAsync(cancelattiontoken, page, pageSize, situacao);

                if(pedidosCozinha == null)
                {
                    return NotFound("Pedido Cozinha não encontrado");
                }
                return Ok(pedidosCozinha);
            }
            catch(Exception ex)
            {
                _logger.LogError("[{1},{2}] Iniciando consulta de pedidos", nameof(GetPedidosAsync), ex.Message);
                return StatusCode(500, "Ocorreu um erro interno");
            }

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
