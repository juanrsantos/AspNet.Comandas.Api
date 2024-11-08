using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comandas.Api.Models
{
    public class CardapioItem
    {
        // Key = significa chave primaria
        // DatabaseGenerated = Valor Gerado da coluna será realizado pelo BD
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Titulo { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public decimal Preco { get; set; }
        public bool PossuiPreparo { get; set; }
    }
}
