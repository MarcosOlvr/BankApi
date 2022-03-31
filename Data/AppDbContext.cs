using Cumbuca.Models;
using Microsoft.EntityFrameworkCore;

namespace Cumbuca.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Transacao>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.ContaEnviante)
                .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Transacao>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.ContaRecebedora)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasIndex(u => u.Cpf).IsUnique();
        }
    }
}
