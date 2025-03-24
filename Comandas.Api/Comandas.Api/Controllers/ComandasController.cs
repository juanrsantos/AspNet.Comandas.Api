using Comandas.Api.Enums;
using Comandas.Data;
using Comandas.Domain;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

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

            try
            {
                await _comandaServices.UpdateComandaAsync(comanda);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
               return StatusCode(500, "Ocorreu um erro interno");
            }
           
            return NoContent(); // 204
        }

     

        // DELETE api/<ComandasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
