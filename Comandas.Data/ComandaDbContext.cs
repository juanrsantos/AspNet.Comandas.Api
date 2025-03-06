using Comandas.Domain;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Data
{
    public class ComandaDbContext(DbContextOptions<ComandaDbContext> options) : DbContext(options)
    {

        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<CardapioItem> CardapioItems { get; set; }
        public DbSet<Comanda> Comandas { get; set; }
        public DbSet<ComandaItem> ComandaItems { get; set; }
        public DbSet<PedidoCozinha> PedidoCozinhas { get; set; }
        public DbSet<PedidoCozinhaItem> PedidoCozinhaItems { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

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
                .HasForeignKey(pci => pci.PedidoCozinhaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PedidoCozinhaItem>()
                .HasOne(tico => tico.PedidoCozinha)
                .WithMany(tico => tico.PedidoCozinhaItems)
                .HasForeignKey(teco => teco.PedidoCozinhaId)
                .OnDelete(DeleteBehavior.NoAction);
            

            modelBuilder.Entity<PedidoCozinhaItem>()
                .HasOne(pci => pci.ComandaItem)
                .WithMany()
                .HasForeignKey(pci => pci.ComandaItemId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
