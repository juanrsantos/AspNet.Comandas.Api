using Comandas.Api.Data;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        private readonly ComandaDbContext _context;

        public MesasController(ComandaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mesa>>> GetMesas(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Mesas.ToListAsync(cancellationToken);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = "Parâmetro inválido fornecido." });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, new { message = "A operação foi cancelada." });
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor, tente novamente");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mesa>> GetMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa is null)
            {
                return NotFound();
            }
            return mesa;
          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesa(int id, Mesa mesa)
        {
            if(id != mesa.Id)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do corpo da requisição ");
            }
            var existeMesa = MesaExists(id);
            if(!existeMesa)
            {
                return NotFound(new { message = $"Mesa com ID {id} não encontrada." });
            }

            _context.Entry(mesa).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!MesaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Ocorreu um erro ao conectar no banco");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa is null)
            {
                return NotFound();
            }

            _context.Mesas.Remove(mesa);
            _context.SaveChanges();
            return NoContent();
        }


        private bool MesaExists(int id) 
        { 
            return _context.Mesas.Any(mesa => mesa.Id == id);   
        }
    }
}
