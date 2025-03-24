using Comandas.Domain;
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
    }
}
