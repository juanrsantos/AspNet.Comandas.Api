using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comandas.Api.Models
{
    public class Mesa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public int SituacaoMesa { get; set; }
    }
}
