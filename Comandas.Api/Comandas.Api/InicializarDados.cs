using Comandas.Api.Data;

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
                    Id=1,
                    NumeroMesa = 123,
                    SituacaoMesa = 1
                    
                });
            }

            if (!context.CardapioItems.Any())
            {
                context.CardapioItems.AddRange(new Models.CardapioItem
                {
                 Id = 1,
                 PossuiPreparo = true,
                 Preco = 50,
                 Descricao = "Prato Principal",
                 Titulo = "Xtudo"
                });
            }




            context.SaveChanges();
        }

    }
}
