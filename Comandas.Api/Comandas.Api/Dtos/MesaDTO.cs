using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comandas.Api.Dtos
{
    public class MesaDTO
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public int SituacaoMesa { get; set; }
    }
}
