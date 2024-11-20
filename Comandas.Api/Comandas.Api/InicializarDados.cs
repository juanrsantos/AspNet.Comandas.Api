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
            context.SaveChanges();
        }

    }
}
