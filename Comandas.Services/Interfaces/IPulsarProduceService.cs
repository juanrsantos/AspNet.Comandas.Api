using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Services.Interfaces
{
    public interface IPulsarProduceService
    {
        Task EnviarMensagemAsync(EventoUsuario eventoUsuario);
    }
}
