namespace Comandas.Api.Dtos
{
    public class ComandaUpdateDTO
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        public ComandaItemUpdateDTO[] ComandaItems { get; set; } = [];


    }

    public class ComandaItemUpdateDTO
    {
        public int Id { get; set; }
        public int CardapioItemId { get; set; }
        public bool Incluir { get; set; }
        public bool Excluir { get; set; }
    }
}
