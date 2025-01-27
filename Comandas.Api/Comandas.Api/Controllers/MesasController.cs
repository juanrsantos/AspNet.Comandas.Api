using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Policy;

namespace Comandas.Api.Controllers
{
    [Tags("02. Mesas ")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MesasController : ControllerBase
    {
        private readonly ComandaDbContext _context;
        private readonly ILogger<MesasController> _logger;

        public MesasController(ComandaDbContext context, ILogger<MesasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Obtém todas as mesas", Description = "Lista todos as mesas cadastrado")]
        [SwaggerResponse(200, "retorna a lista de mesas", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Mesas não encontrada", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<Mesa>>> GetMesas(CancellationToken cancellationToken, int page, int pageSize)
        {
            _logger.LogInformation($"[{nameof(GetMesas)}] Iniciando consulta de mesas");
            try
            {
                var query = _context.Mesas.AsQueryable();
                var count = await query.CountAsync();

                var mesas = await query.Skip((page - 1) * pageSize).Take(pageSize)
                    .TagWith("GetMesas").AsNoTracking().Select(x => new MesaDTO
                    {
                        Id = x.Id,
                        NumeroMesa = x.NumeroMesa,
                        SituacaoMesa = x.SituacaoMesa
                    }).ToListAsync(cancellationToken);


                if (mesas == null || count == 0)
                {
                    return NotFound("Mesas não encontrada");
                }

                var res = new PagedResponseDto<MesaDTO>(mesas, count, page, pageSize);
                _logger.LogInformation($"[{nameof(GetMesa)}] Mesas obtida com sucesso");
                return Ok(res);

            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Erro");
                return BadRequest(new { message = "Parâmetro inválido fornecido." });
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Erro");
                return StatusCode(499, new { message = "A operação foi cancelada." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro");
                return StatusCode(500, "Ocorreu um erro interno no servidor, tente novamente");
            }
        }



        [SwaggerOperation(Summary = "Obtém uma unica mesa", Description = "Obtém a mesa por Id")]
        [SwaggerResponse(200, "retorna uma unica mesa", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Mesa não encontrada", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
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

        [SwaggerOperation(Summary = "Cria uma nova mesa", Description = "Cria uma nova mesa")]
        [SwaggerResponse(201, "retorna Id da mesa criada", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpPost]
        public async Task<ActionResult<Mesa>> PostMesa(Mesa mesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMesa), new { id = mesa.Id }, mesa);
        }

        [SwaggerOperation(Summary = "Edita uma mesa", Description = "Edita uma mesa")]
        [SwaggerResponse(204, "Mesa Editada", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
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

        [SwaggerOperation(Summary = "Exclui uma mesa", Description = "Exclui uma mesa")]
        [SwaggerResponse(204, "Mesa Excluida", typeof(IEnumerable<Mesa>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
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
