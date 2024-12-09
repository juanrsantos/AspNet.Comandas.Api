namespace Comandas.Api.Dtos
{
    public class ComandaGetDTO
    {
        public int Id { get; set; }

        public int NroMesa { get; set; }
        public string  NomeCliente { get; set; }

        public IList<ComandaItemGetDto> comandaItems { get; set; } = new List<ComandaItemGetDto>();


    }

    public class ComandaItemGetDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
    }
}
