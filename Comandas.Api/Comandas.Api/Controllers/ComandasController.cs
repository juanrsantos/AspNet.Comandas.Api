using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Enums;
using Comandas.Api.Models;
using Comandas.Api.Services.Implementation;
using Comandas.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Tags("04. Comandas")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComandasController : ControllerBase
    {
        private readonly ComandaDbContext _context;
        private readonly IComandasServices  _comandaServices;
        private readonly ILogger<ComandasController> _logger;

        public ComandasController(IComandasServices applicationComandasService, ILogger<ComandasController> logger, ComandaDbContext context)
        {
            _comandaServices = applicationComandasService;
            _logger = logger;
            _context = context;
        }

        // GET: api/<ComandasController>
        [SwaggerOperation(Summary = "Obtém todas as comandas", Description = "Lista todas as comandas")]
        [SwaggerResponse(200, "retorna a lista de comandas", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<ComandaGetDTO>>> GetComandas(CancellationToken cancellationToken, int page, int pageSize)
        {
            _logger.LogInformation($"[{nameof(GetComandas)}] Iniciando consulta de comandas");
            try
            {
                var comandas = await _comandaServices.GetComandasAsync(cancellationToken, page, pageSize);

                if (comandas == null || comandas.TotalRegistros == 0)
                {
                    return NotFound("Mesas não encontrada");
                }
                _logger.LogInformation($"[{nameof(GetComandas)}] Comandas obtida com sucesso");
                return Ok(comandas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro");
                return StatusCode(500, "Ocorreu um erro interno no servidor, tente novamente");

            }
        }

        // GET api/<ComandasController>/5
        [SwaggerOperation(Summary = "Obtém a comanda", Description = "Obtém a comanda por Id")]
        [SwaggerResponse(200, "retorna a comanda especifica", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ComandaGetDTO>> Get(int id)
        {
            var comanda = await _comandaServices.Get(id);
         
            return Ok(comanda);
        }

        // POST api/<ComandasController>
        [SwaggerOperation(Summary = "Cria uma nova comanda", Description = "Cria uma nova comanda")]
        [SwaggerResponse(201, "retorna o id do objeto criado", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Comanda>> Post([FromBody] ComandaDTO comanda)
        {
            try
            {
                var comandaResponse = await _comandaServices.Post(comanda);
                // Devolvendo no cabeçalho da resposta a url de consulta do novo objeto gerado.
                return CreatedAtAction(nameof(Get), new { id = comandaResponse.Id }, comanda);
            }
            catch (BadRequestException ex)
            {
                // Retornar um erro 400 com a mensagem personaliada
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Em caso de erro inesperado, um erro generico.
                return StatusCode(500, "Ocorreu um erro interno");
            }
       
        }

        // PUT api/<ComandasController>/5
        [SwaggerOperation(Summary = "Edita comanda", Description = "Edita uma comanda")]
        [SwaggerResponse(204, "Comanda editada ", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ComandaUpdateDTO comanda)
        {
            //var comandaUpdates = await _comandaServices.Get(id);
            var comandaUpdate = await _context.Comandas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (comandaUpdate is null)
            {
                return NotFound("Não encontrado");
            }

            if (!string.IsNullOrEmpty(comanda.NomeCliente))
            {
                comandaUpdate.NomeCliente = comanda.NomeCliente;
            }
            if (comanda.NumeroMesa > 0)
            {
                // Verificar disponibilidade da mesa
                var mesa = await _context.Mesas.AsNoTracking().FirstOrDefaultAsync(x => x.NumeroMesa == comanda.NumeroMesa);

                if (mesa is null)
                {
                    return BadRequest("Mesa invalida");
                }
                if (mesa.SituacaoMesa != (int)SituacaoMesaEnum.Disponivel)
                {
                    return BadRequest("Mesa ocupada");
                }
                // Mudar status da mesa para ocupado
                mesa.SituacaoMesa = (int)SituacaoMesaEnum.Ocupado;
                // Mudar o status da mesa antiga para disponivel 
                var mesaantiga = await _context.Mesas.AsNoTracking().FirstOrDefaultAsync(x => x.NumeroMesa == comandaUpdate.NumeroMesa);
                mesaantiga.SituacaoMesa = (int)SituacaoMesaEnum.Disponivel;
                // Atualizar o numero da mesa comanda
                comandaUpdate.NumeroMesa = comanda.NumeroMesa;

            }

            // percorrer os itens da comanda
            foreach (var item in comanda.ComandaItems)
            {
                // verificar se esta incluindo itens 
                if (item.Incluir)
                {
                    var novoComandaItem = new ComandaItem
                    {
                        Comanda = comandaUpdate,
                        CardapioItemId = item.CardapioItemId,
                    };
                    await _context.ComandaItems.AddAsync(novoComandaItem);

                    // Verificar se o cardapio possui preparo
                    var cardapioItem = await _context.CardapioItems.FirstOrDefaultAsync(x => x.Id == item.CardapioItemId);
                    if (cardapioItem is null)
                    {
                        return BadRequest("Cardapio invalido");
                    }
                    // criar o pedido de cozinha
                    if (cardapioItem.PossuiPreparo)
                    {
                        var novoPedidoCozinha = new PedidoCozinha
                        {
                            Comanda = comandaUpdate,
                            SituacaoId = 1
                        };

                        await _context.PedidoCozinhas.AddAsync(novoPedidoCozinha);
                        // criar o item do pedido de cozinha

                        var novoPedidoCozinhaItem = new PedidoCozinhaItem
                        {
                            PedidoCozinha = novoPedidoCozinha,
                            ComandaItem = novoComandaItem,
                        };
                        await _context.PedidoCozinhaItems.AddAsync(novoPedidoCozinhaItem);
                    }
                }

                // verificar se esta excluindo itens 
                if (item.Excluir)
                {
                    var comandaItemExcluir = await _context.ComandaItems.AsNoTracking().FirstAsync(x => x.Id == item.Id);
                    _context.ComandaItems.Remove(comandaItemExcluir);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existeComanda = await ComandaExiste(id);
                if (!existeComanda)
                {
                    return NotFound("Comanda não encontrada");
                }

                throw;
            }

            return NoContent(); // 204
        }

        private async Task<bool> ComandaExiste(int id)
        {
            return await _context.Comandas.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        // DELETE api/<ComandasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
