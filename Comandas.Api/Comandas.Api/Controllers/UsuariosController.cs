using Comandas.Data;
using Comandas.Domain;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Comandas.Api.Controllers
{
    [Tags("01. Usuários")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ComandaDbContext _context;
        private readonly IUsuarioServices _usuarioServices;

        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ComandaDbContext context, ILogger<UsuariosController> logger, IUsuarioServices usuarioServices)
        {
            _context = context;
            _logger = logger;
            _usuarioServices = usuarioServices;
        }
        /// <summary>
        /// Realiza o login da aplicação
        /// </summary>
        /// <param name="usuarioRequest"></param>
        /// <returns>200ok</returns>
        [SwaggerOperation(Summary = "Gera o token de autenticação da Api", Description = "Realiza o login do usuário na aplicação, necessário informar email e senha")]
        [SwaggerResponse(200,"retorna o nome do usuário e o token de autenticação",typeof(UsuarioResponse))]
        [SwaggerResponse(404, "usuário não encontrado", typeof(string))]
        [SwaggerResponse(400, "Usuário/Senha invalida", typeof(string))]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponse>> Login([FromBody] UsuarioRequest usuarioRequest, CancellationToken cancellationToken)
        {
            try
            {
                var login = await _usuarioServices.Login(usuarioRequest, cancellationToken);
                return Ok(login);
            }
            catch (NotFoundException ex) 
            {
                _logger.LogError("Usuario não encontrado", ex.Message);
                return NotFound(ex.Message);
            }
            catch(BadRequestException ex)
            {
                _logger.LogError("Erro");
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int) HttpStatusCode.InternalServerError, "Erro generico");
            }
        }

        [SwaggerOperation(Summary ="Obtém todos usuários", Description ="Lista todos os usuários cadastrados")]
        [SwaggerResponse(200, "retorna a lista de usuários", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Usuários não encontado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedResponseDto<UsuarioDTO>>> GetUsuarios(int page, int pageSize)
        {
            _logger.LogInformation($"[{nameof(GetUsuarios)}] Iniciando consulta de usuários");
            try
            {

                var query =  _context.Usuarios.AsQueryable();
                var count = await query.CountAsync();

                var usuarios = await query.Skip((page - 1) * pageSize).Take(pageSize)

                    .TagWith("GetUsuarios").AsNoTracking().Select(x => new UsuarioDTO
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Email = x.Email
                    }).ToListAsync();


                if(usuarios == null || count == 0)
                {
                    return NotFound("Usuários não encontrado");
                }

                var res = new PagedResponseDto<UsuarioDTO>(usuarios, count, page, pageSize);
              

                _logger.LogInformation($"[{nameof(GetUsuarios)}] Usuários obtido com sucesso");
                return Ok(res);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar o banco");
            }
            catch (Exception ex)
            {
                // Log da exceção (ex) pode ser realizado aqui
                _logger.LogError(ex, "Erro"); 
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar usuários. Por favor, tente novamente mais tarde. ");
            }

        }

        [SwaggerOperation(Summary = "Obtém um unico usuário", Description = "Realiza o get de um unico usuario, atraves do ID")]
        [SwaggerResponse(200, "retorna o usuário", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Usuário não encontado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (usuario is null)
                {
                    return NotFound();
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar ao banco");
            }
     
        }

        [SwaggerOperation(Summary = "Edita usuário", Description = "Realiza alteração nos campo do usuário")]
        [SwaggerResponse(204, "Usuário alterado", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        [SwaggerOperation(Summary = "Excluir usuário", Description = "Realiza exclusão do usuário")]
        [SwaggerResponse(204, "Usuário excluido", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if(usuario is null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(us => us.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }
    }
}
