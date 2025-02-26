using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comandas.Api.Models
{
    public class Usuario
    {
        // Coluna ID é a chave primaria
        // Auto incremento: 1,2,3,4
        // Anotação
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress(ErrorMessage ="Email inválido")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)] 
        public string Senha { get; set; } = string.Empty;
    }
}
