using Comandas.Api.Data;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Controllers
{
    [Tags("05. Cardapio Itens")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardapioItensController : ControllerBase
    {
        private readonly ComandaDbContext _context;

        public CardapioItensController(ComandaDbContext context)
        {
            _context = context; 
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardapioItem>>> GetCardapioItem()
        {
            try
            {
                return await _context.CardapioItems.AsNoTracking().ToListAsync();
            }
            catch (Exception) 
            {
                return BadRequest("Erro ao conectar banco de dados");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardapioItem>> GetCardapioItem(int id)
        {
            var cardapioitem = await _context.CardapioItems.FindAsync(id);

            if(cardapioitem is null)
            {
                return NotFound();  
            }
            return cardapioitem;
        }

        [HttpPost]
        public async Task<ActionResult<CardapioItem>> PostCardapioItem(CardapioItem cardapioitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.CardapioItems.Add(cardapioitem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCardapioItem), new { id = cardapioitem.Id }, cardapioitem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardapioItem(int id , CardapioItem cardapioitem)
        {
            if(id != cardapioitem.Id)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do corpo da requisição ");
            }
     
            _context.Entry(cardapioitem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardapioItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao conectar no banco");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesaItem(int id)
        {
            var cardapioItem =await _context.CardapioItems.FindAsync(id);
            if(cardapioItem is null){
                return NotFound();
            }

            _context.CardapioItems.Remove(cardapioItem);
            _context.SaveChanges();
            return NoContent();
        }
        private bool CardapioItemExists (int id)
        {
            return _context.CardapioItems.AsNoTracking().Any(x => x.Id == id);
        }
    }
}
