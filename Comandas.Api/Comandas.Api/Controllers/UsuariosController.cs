using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ComandaDbContext context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
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
        public async Task<ActionResult<UsuarioResponse>> Login([FromBody] UsuarioRequest usuarioRequest)
        {
            var tokengerador = new JwtSecurityTokenHandler();
            var chave = Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b");
            // Consultar usuario no banco 
            // TagWith colocar um cabeçalho no log de saida do sql.
            // AsNoTracking serve para não rastrear modificações na tabela, tornando a consulta mais leve. 
            var usuario = await _context.Usuarios.TagWith("Login").AsNoTracking().FirstOrDefaultAsync(x => x.Email == usuarioRequest.Email);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            if (!usuario.Senha.Equals(usuarioRequest.Senha))
            {
                return BadRequest("Usuário/Senha invalida");
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature),
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                    }
                    )
            };

            var token = tokengerador.CreateToken(tokenDescriptor);
            var tokenfinal = tokengerador.WriteToken(token);

            return Ok(new UsuarioResponse { Nome = usuario.Nome , Token= tokenfinal });
        }
        [SwaggerOperation(Summary ="Obtém todos usuários", Description ="Lista todos os usuários cadastrados")]
        [SwaggerResponse(200, "retorna a lista de usuários", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(401, "Acesso não autorizado", typeof(string))]
        [SwaggerResponse(404, "Usuários não encontado", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error")]

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            _logger.LogInformation($"[{nameof(GetUsuarios)}] Iniciando consulta de usuários");
            try
            {
                var usuarios = await _context.Usuarios.TagWith("GetUsuarios").AsNoTracking().ToListAsync();
                if(usuarios == null || !usuarios.Any())
                {
                    return NotFound("Usuários não encontrado");
                }
                _logger.LogInformation($"[{nameof(GetUsuarios)}] Usuários obtido com sucesso");
                return Ok(usuarios);
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
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

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
    }
}
