using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
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
    }
}
