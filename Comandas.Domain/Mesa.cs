using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Comandas.Shared.Exceptions;
using Comandas.Api.Enums;


namespace Comandas.Api.Models
{
    public class Mesa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public int SituacaoMesa { get; set; }

        public void AlterarSituacao(int situacaoMesa)
        {
            VerificarSituacao();
            SituacaoMesa = situacaoMesa;    
        }

        private void VerificarSituacao()
        {
            if (SituacaoMesa != (int)SituacaoMesaEnum.Disponivel)
            {
                throw new BadRequestException("Mesa ocupada");
            }
        }
    }
}
