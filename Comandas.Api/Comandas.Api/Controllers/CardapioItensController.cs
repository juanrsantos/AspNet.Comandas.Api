﻿using Comandas.Api.Data;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return await _context.CardapioItems.ToListAsync();
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
            return _context.CardapioItems.Any(x => x.Id == id);
        }
    }
}