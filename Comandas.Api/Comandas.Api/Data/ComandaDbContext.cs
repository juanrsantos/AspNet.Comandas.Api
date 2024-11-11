using Comandas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Api.Data
{
    public class ComandaDbContext :DbContext
    {
        public ComandaDbContext(DbContextOptions<ComandaDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent-Api a Comanda tem varias comandaItems e varias comandas itens tem uma Comanda e a foreing key e comandaID
            modelBuilder.Entity<Comanda>()
                .HasMany<ComandaItem>()
                .WithOne(Ci => Ci.Comanda)
                .HasForeignKey(f => f.ComandaId);

            modelBuilder.Entity<ComandaItem>()
                .HasOne(ci => ci.Comanda)
                .WithMany(ci => ci.ComandaItems)
                .HasForeignKey(f => f.ComandaId);

            modelBuilder.Entity<ComandaItem>()
                .HasOne(ci => ci.CardapioItem)
                .WithMany()
                .HasForeignKey(f => f.CardapioItemId);

            modelBuilder.Entity<PedidoCozinha>()
                .HasMany<PedidoCozinhaItem>()
                .WithOne(pci => pci.PedidoCozinha)
                .HasForeignKey(pci => pci.PedidoCozinhaId);

            modelBuilder.Entity<PedidoCozinhaItem>()
                .HasOne(tico => tico.PedidoCozinha)
                .WithMany(tico => tico.PedidoCozinhaItems)
                .HasForeignKey(teco => teco.PedidoCozinhaId);

            modelBuilder.Entity<PedidoCozinhaItem>()
                .HasOne(pci => pci.ComandaItem)
                .WithMany()
                .HasForeignKey(pci => pci.ComandaItemId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
