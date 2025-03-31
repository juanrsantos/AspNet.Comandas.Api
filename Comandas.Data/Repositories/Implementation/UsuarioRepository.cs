using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Data.Repositories.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ComandaDbContext _context;

        public UsuarioRepository(ComandaDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetUsuarioByEmail(string email, CancellationToken cancellationToken)
        {
            // Consultar usuario no banco 
            // TagWith colocar um cabeçalho no log de saida do sql.
            // AsNoTracking serve para não rastrear modificações na tabela, tornando a consulta mais leve. 

            return await _context.Usuarios.TagWith("Login").AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken); 
        }

        public async Task<PagedResponseDto<UsuarioDTO>> GetUsuariosAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Usuarios.AsQueryable();
                var count = await query.CountAsync();

                var usuarios = await query.Skip((page - 1) * pageSize).Take(pageSize)
                    .TagWith("GetUsuarios").AsNoTracking().Select(x => new UsuarioDTO
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Email = x.Email
                    }).ToListAsync();


                if (usuarios == null || count == 0)
                {
                    throw new NotFoundException("Usuários não encontrado");
                }

                return  new PagedResponseDto<UsuarioDTO>(usuarios, count, page, pageSize);
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("Erro ao acessar o banco");
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Erro ao buscar usuários. Por favor, tente novamente mais tarde. ");
            }
        }
    }
}
