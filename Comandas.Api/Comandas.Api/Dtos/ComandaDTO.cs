namespace Comandas.Api.Dtos
{
    public class ComandaDTO
    {
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        public int[] CardapioItems { get; set; } = [];


    }
}
