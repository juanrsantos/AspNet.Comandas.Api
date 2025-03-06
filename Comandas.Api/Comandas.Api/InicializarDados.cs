using Comandas.Data;

namespace Comandas.Api
{
    public static class InicializarDados
    {
        public static void Semear(ComandaDbContext context) 
        {

            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(new Comandas.Domain.Usuario
                {
                    Nome = "Juan",
                    Email = "juan.rodrigues.santos@hotmail.com",
                    Senha = "123456"
                });
            }

            if (!context.Mesas.Any())
            {
                context.Mesas.AddRange(new Comandas.Domain.Mesa
                {
                    NumeroMesa = 123,
                    SituacaoMesa = 1
                    
                });
            }

            if (!context.CardapioItems.Any())
            {
                context.CardapioItems.AddRange(new Comandas.Domain.CardapioItem
                {
                 PossuiPreparo = true,
                 Preco = 50,
                 Descricao = "Prato Principal",
                 Titulo = "Xtudo"
                });
            }

            if (!context.Mesas.Any())
            {
                context.Mesas.AddRange(new Comandas.Domain.Mesa
                {
                    NumeroMesa = 1,
                    SituacaoMesa = 0
                }, new Comandas.Domain.Mesa
                {
                    NumeroMesa = 2,
                    SituacaoMesa = 0

                }, new Comandas.Domain.Mesa
                {
                    NumeroMesa = 3,
                    SituacaoMesa = 1
                }, new Comandas.Domain.Mesa
                {
                    NumeroMesa = 4,
                    SituacaoMesa = 0
                });
            }

            if (!context.Comandas.Any())
            {
                var comanda = new Comandas.Domain.Comanda
                {
                    NomeCliente = "Juan Rodrigues",
                    NumeroMesa = 3,
                    SituacaoComanda = 1
                };

                context.Comandas.Add(comanda);

                Comandas.Domain.ComandaItem[] comandaitems =
                {
                    new Comandas.Domain.ComandaItem
                    {
                        Comanda = comanda,
                        CardapioItemId = 1
                    }
                };
                if (!context.ComandaItems.Any())
                {
                    //context.ComandaItems.AddRange(comandaitems);
                }

            }


            context.SaveChanges();
        }

    }
}
