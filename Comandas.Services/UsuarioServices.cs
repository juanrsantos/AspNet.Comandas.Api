using Comandas.Data.Repositories.Implementation;
using Comandas.Data.Repositories.Interfaces;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPulsarProduceService _pusarProduceService;

        public UsuarioServices(IUsuarioRepository usuarioRepository, IPulsarProduceService pusarProduceService)
        {
            _usuarioRepository = usuarioRepository;
            _pusarProduceService = pusarProduceService;
        }

        public async Task<PagedResponseDto<UsuarioDTO>> GetUsuariosAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _usuarioRepository.GetUsuariosAsync(page, pageSize, cancellationToken);
        }

        public async Task<UsuarioResponse> Login(UsuarioRequest usuarioRequest, CancellationToken cancellationToken)
        {
            var tokengerador = new JwtSecurityTokenHandler();
            var chave = Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b");

            var usuario = await _usuarioRepository.GetUsuarioByEmail(usuarioRequest.Email, cancellationToken);

            if (usuario == null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            if (!usuario.Senha.Equals(usuarioRequest.Senha))
            {
                throw new BadRequestException("Usuário/Senha invalida");
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

            await _pusarProduceService.EnviarMensagemAsync(new EventoUsuario
            {
                Acao = "Login",
                Assunto = "Logado com sucesso",
                Email = usuario.Email
            });

            return new UsuarioResponse { Nome = usuario.Nome, Token = tokenfinal };
        }
    }
}
