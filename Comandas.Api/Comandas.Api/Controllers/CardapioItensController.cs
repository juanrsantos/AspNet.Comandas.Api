using Comandas.Data;
using Comandas.Domain;
using Comandas.Services;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Net;

namespace Comandas.Api.Controllers
{
    [Tags("05. Cardapio Itens")]
    [Route("api/[controller]")]
    [ApiController]
//  [Authorize]
    public class CardapioItensController : ControllerBase
    {
        private readonly CardapioItensServices _cardapioItensServices;
        private readonly ILogger<CardapioItensController> _logger;

        public CardapioItensController(CardapioItensServices cardapioItensServices)
        {
            _cardapioItensServices = cardapioItensServices;
        }


        [SwaggerOperation(Summary = "Obtém todos cardapios items", Description ="Obtem todos")]
        [SwaggerResponse(200, "Retorna o cardapio item especifico", typeof(CardapioItem))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<CardapioItem>>> GetCardapioItens(int page, int pageSize, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[{nameof(GetCardapioItem)}] Iniciando a consulta de todos cardapios");
            try
            {
                var listaCardapiosItem = await _cardapioItensServices.GetCardapioItensAsync(page, pageSize, cancellationToken);
                return Ok(listaCardapiosItem);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Itens não encontrado");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado");
                return StatusCode(500, "Erro interno");
            }

        }

        [SwaggerOperation(Summary = "Obtem o cardapio item", Description = "Obtém a comanda Por ID")]
        [SwaggerResponse(200, "Retorna o cardapio item especifico", typeof(CardapioItem))]
        [SwaggerResponse(400, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CardapioItemDTO>> GetCardapioItem(int id)
        {
            _logger.LogInformation($"[{nameof(GetCardapioItem)}] Iniciando a consulta de Cardapio");
            try
            {
                var cardapioitem = await _cardapioItensServices.GetCardapioItem(id);

                if(cardapioitem == null)
                {
                    _logger.LogWarning($"Cardápio item com ID{id} não encontrado");
                    return NotFound("Item de cardápio não encontrado");
                }
                return Ok(cardapioitem);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError("Bad Request - Ocorreu um erro ao processar a requisição do cardápio.");
                return BadRequest("Requisição inválida.");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Item de cardápio não encontrado.");
                return NotFound(ex.Message);
            }
       
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao processar a requisição.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CardapioItem>> PostCardapioItem(CardapioItemDTO cardapioitem)
        {
            _logger.LogInformation($"{nameof(CardapioItem)} Iniciando adição de novo CardapioItem");
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var cardapioItemCriado = await _cardapioItensServices.AddCardapioItemAsync(cardapioitem);
                return CreatedAtAction(nameof(GetCardapioItem), new { id = cardapioItemCriado.Id }, cardapioItemCriado);
            }
            catch(BadRequestException ex)
            {
                _logger.LogError(ex, "Dados invalidos ao adicionar cardapio");
                return BadRequest(ex.Message);

            }
            catch(RepositoryException ex)
            {
                _logger.LogError(ex, "Erro no banco de dados ao adicionar");
                return StatusCode(500, "Erro interno no banco");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar CardapioItem.");
                return StatusCode(500, "Erro interno inesperado.");
            }

        }

        [SwaggerOperation(Summary = "Obtem o cardapio item", Description = "Obtém a comanda Por ID")]
        [SwaggerResponse(204, "Retorna o cardapio item especifico", typeof(CardapioItem))]
        [SwaggerResponse(400, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardapioItem(int id , CardapioItemDTO cardapioitem)
        {
            _logger.LogInformation($"{nameof(CardapioItem)} Atualizando CardapioItem");
            try
            {
                if (id != cardapioitem.Id)
                {
                    return BadRequest("Id do item não é o mesmo do corpo da url");
                }

                await _cardapioItensServices.UpdateCardapioItemAsync(cardapioitem);
                return NoContent(); // aTUALIZÇAÃO BEM SUCESIDADE 204 
            }
            catch(NotFoundException ex)
            {
                _logger.LogWarning(ex, "Item para atualização não encontrado");
                return NotFound(ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Conflito de concorrência ao atualizar CardapioItem.");
                return Conflict(ex.Message); // 409 Conflict
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Erro no repositório.");
                return StatusCode(500, "Erro interno no banco de dados.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                return StatusCode(500, "Erro interno inesperado.");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesaItem(int id)
        {
            _logger.LogInformation($"[{nameof(DeleteMesaItem)}] Iniciando exclusão do CardapioItem com ID {id}.");
            try
            {
                
                await _cardapioItensServices.ExcluirCardapioItemAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Item para exclusão não encontrado.");
                return NotFound(ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Conflito de concorrência ao excluir CardapioItem.");
                return Conflict(ex.Message); // 409 Conflict
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Erro no repositório ao excluir CardapioItem.");
                return StatusCode(500, "Erro interno no banco de dados.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao excluir CardapioItem.");
                return StatusCode(500, "Erro interno inesperado.");
            }

        }
    }
}
