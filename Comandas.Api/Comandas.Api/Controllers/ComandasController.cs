using Comandas.Api.Data;
using Comandas.Api.Dtos;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ComandasController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
        public void Put(int id, [FromBody] ComandaUpdateDTO comanda)
        {

        }

        // DELETE api/<ComandasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
