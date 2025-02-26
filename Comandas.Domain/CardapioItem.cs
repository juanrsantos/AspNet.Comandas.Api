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

        [StringLength(150)]
        public string Titulo { get; set; } = default!;

        [StringLength(300)]
        public string Descricao { get; set; } = default!;

        [Column(TypeName ="decimal(10,2)")]
        public decimal Preco { get; set; }
        public bool PossuiPreparo { get; set; }
    }
}
