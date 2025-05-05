namespace Comandas.Services.Interfaces
{
    public class EventoUsuario
    {
        public required string Acao { get; set; }
        public required string Email { get; set; }
        public required string Assunto { get; set; }

    }
}