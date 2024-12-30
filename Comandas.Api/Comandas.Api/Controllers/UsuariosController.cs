using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ComandaDbContext _context;

        public UsuariosController(ComandaDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponse>> Login([FromBody] UsuarioRequest usuarioRequest)
        {
            var tokengerador = new JwtSecurityTokenHandler();
            var chave = Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b");
            // Consultar usuario no banco 
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioRequest.Email);

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


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                return NotFound();
            }
            return usuario;
        }

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
