using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Shared.Dtos
{
    public class CardapioItemDTO
    {

        public int Id { get; set; }

        public string Titulo { get; set; } = default!;

        public string Descricao { get; set; } = default!;

        public decimal Preco { get; set; }
        public bool PossuiPreparo { get; set; }
    }
}
