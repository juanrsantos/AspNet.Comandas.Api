using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Enums;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComandasController : ControllerBase
    {

        private readonly ComandaDbContext _context;
        public ComandasController(ComandaDbContext contexto)
        {
            _context = contexto;
        }

        // GET: api/<ComandasController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComandaGetDTO>>> Get()
        {
            var comandas = await _context.Comandas
                .Where(x => x.SituacaoComanda == 1)
                .Select(x => new ComandaGetDTO
                {
                    Id = x.Id,
                    NomeCliente = x.NomeCliente,
                    NroMesa = x.NumeroMesa,
                    comandaItems = x.ComandaItems.Select(x => new ComandaItemGetDto
                    {
                        Id = x.Id,
                        Titulo = x.CardapioItem.Titulo,
                    }).ToList()
                }).ToListAsync();
      
            return Ok(comandas);
        }

        // GET api/<ComandasController>/5
        [HttpGet("{id}")]
        public async Task <ActionResult<ComandaGetDTO>> Get(int id)
        {
            var comanda = await _context.Comandas.FirstOrDefaultAsync(x => x.Id == id);
            if(comanda is null)
            {
                return NotFound($"Comanda {id} não encontrada ");
            }

            var comandaDTO = new ComandaGetDTO
            {
                NroMesa = comanda.NumeroMesa,
                NomeCliente =comanda.NomeCliente,
            };

            var comandaItemsDto = await _context.ComandaItems.Include(x => x.CardapioItem)
                .Where(x => x.ComandaId == id)
                .Select(x => new ComandaItemGetDto
                {
                    Id = x.Id,
                    Titulo = x.CardapioItem.Titulo
                }).ToListAsync();

            comandaDTO.comandaItems = comandaItemsDto;
            return Ok(comandaDTO);
        }

        // POST api/<ComandasController>
        [HttpPost]
        public async Task<ActionResult<Comanda>> Post([FromBody] ComandaDTO comanda)
        {
            var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.NumeroMesa == comanda.NumeroMesa);

            if (mesa is null) {
                return BadRequest("Mesa não encontrada");
            }

            if(mesa.SituacaoMesa != 0)
            {
                return BadRequest("Mesa ocupada");
            }

            mesa.SituacaoMesa = 1;
            var novaComanda = new Comanda
            {
                NumeroMesa = comanda.NumeroMesa,
                NomeCliente = comanda.NomeCliente
            };

            await _context.Comandas.AddAsync(novaComanda);

            foreach (var item in comanda.CardapioItems) 
            {
                var novaComandaItem = new ComandaItem
                {
                    Comanda = novaComanda,
                    CardapioItemId = item
                };
                _context.ComandaItems.Add(novaComandaItem);

                // Consultar se o cardapio possui preparo
                var cardapioItem = await _context.CardapioItems.FirstOrDefaultAsync(x => x.Id == item);
                
                if (cardapioItem is null) {
                    return NotFound($"Cardapio com código {item} não encontrado");
                }

                if (cardapioItem.PossuiPreparo)
                {
                    // Se possui preparo criar Pedido de cozinha(Comanda) e Pedido de cozinha item(ComandaItem)
                    var pedidoCozinha = new PedidoCozinha
                    {
                        Comanda = novaComanda,
                        SituacaoId = 1
                    };
                    await _context.PedidoCozinhas.AddAsync(pedidoCozinha);

                    var pedidoCozinhaItem = new PedidoCozinhaItem
                    {
                        PedidoCozinha = pedidoCozinha,
                        ComandaItem = novaComandaItem,
                    };

                    await _context.PedidoCozinhaItems.AddAsync(pedidoCozinhaItem);
                }
            }
            await _context.SaveChangesAsync();

            // Devolvendo no cabeçalho da resposta a url de consulta do novo objeto gerado.
            return CreatedAtAction(nameof(Get), new {id = novaComanda.Id}, comanda);
        }

        // PUT api/<ComandasController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ComandaUpdateDTO comanda)
        {
            var comandaUpdate = await _context.Comandas.FirstOrDefaultAsync(x => x.Id == id);

            if(comandaUpdate is null)
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
                var mesa = await _context.Mesas.FirstOrDefaultAsync(x =>x.NumeroMesa == comanda.NumeroMesa);

                if(mesa is null)
                {
                    return BadRequest("Mesa invalida");
                }
                if(mesa.SituacaoMesa != (int) SituacaoMesaEnum.Disponivel)
                {
                    return BadRequest("Mesa ocupada");
                }
                // Mudar status da mesa para ocupado
                mesa.SituacaoMesa = (int) SituacaoMesaEnum.Ocupado;
                // Mudar o status da mesa antiga para disponivel 
                var mesaantiga = await _context.Mesas.FirstOrDefaultAsync(x => x.NumeroMesa == comandaUpdate.NumeroMesa);
                mesaantiga.SituacaoMesa = (int)SituacaoMesaEnum.Disponivel;
                // Atualizar o numero da mesa comanda
                comandaUpdate.NumeroMesa = comanda.NumeroMesa;

            }

            // percorrer os itens da comanda
            foreach(var item in comanda.ComandaItems)
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
                    if(cardapioItem is null)
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
                    var comandaItemExcluir = await _context.ComandaItems.FirstAsync(x => x.Id == item.Id);
                    _context.ComandaItems.Remove(comandaItemExcluir);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
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
            return await _context.Comandas.AnyAsync(x => x.Id == id);
        }

        // DELETE api/<ComandasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
