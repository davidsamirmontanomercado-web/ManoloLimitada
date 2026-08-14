using ManoloLimitada.Models;
using Microsoft.EntityFrameworkCore;

namespace ManoloLimitada.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contacto>()
                .HasIndex(c => c.Cedula)
                .IsUnique();
        }
    }
}