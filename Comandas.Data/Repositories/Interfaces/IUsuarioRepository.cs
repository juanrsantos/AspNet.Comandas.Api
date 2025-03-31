using Comandas.Domain;
using Comandas.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Data.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetUsuarioByEmail (string email, CancellationToken cancellationToken);

        Task<PagedResponseDto<UsuarioDTO>> GetUsuariosAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
