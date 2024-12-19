using Comandas.Api.Data;
using Comandas.Api.Models;

namespace Comandas.Api
{
    public static class InicializarDados
    {
        public static void Semear(ComandaDbContext context) 
        {

            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(new Models.Usuario
                {
                    Nome = "Juan",
                    Email = "juan.rodrigues.santos@hotmail.com",
                    Senha = "123456"
                });
            }

            if (!context.Mesas.Any())
            {
                context.Mesas.AddRange(new Models.Mesa
                {
                    NumeroMesa = 123,
                    SituacaoMesa = 1
                    
                });
            }

            if (!context.CardapioItems.Any())
            {
                context.CardapioItems.AddRange(new Models.CardapioItem
                {
                 PossuiPreparo = true,
                 Preco = 50,
                 Descricao = "Prato Principal",
                 Titulo = "Xtudo"
                });
            }

            if (!context.Mesas.Any())
            {
                context.Mesas.AddRange(new Models.Mesa
                {
                    NumeroMesa = 1,
                    SituacaoMesa = 0
                }, new Models.Mesa
                {
                    NumeroMesa = 2,
                    SituacaoMesa = 0

                }, new Models.Mesa
                {
                    NumeroMesa = 3,
                    SituacaoMesa = 1
                }, new Models.Mesa
                {
                    NumeroMesa = 4,
                    SituacaoMesa = 0
                });
            }

            if (!context.Comandas.Any())
            {
                var comanda = new Models.Comanda
                {
                    NomeCliente = "Juan Rodrigues",
                    NumeroMesa = 3,
                    SituacaoComanda = 1
                };

                context.Comandas.Add(comanda);

                ComandaItem[] comandaitems =
                {
                    new ComandaItem
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
