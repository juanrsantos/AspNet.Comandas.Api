﻿namespace Comandas.Api.Dtos
{
    public class PedidoCozinhaGetDto
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; } = default!;
        public string Titulo { get; set; } = default!;
    }
}