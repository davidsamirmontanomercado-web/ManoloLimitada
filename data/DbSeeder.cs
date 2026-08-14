using ManoloLimitada.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManoloLimitada.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            // Verificar si ya existe un administrador
            if (await context.Administradores.AnyAsync())
            {
                return;
            }

            var administrador = new Administrador
            {
                Correo = "admin@manolo.com"
            };

            // Hashear la contraseña
            var passwordHasher = new PasswordHasher<Administrador>();

            administrador.Password = passwordHasher.HashPassword(
                administrador,
                "Admin123"
            );

            context.Administradores.Add(administrador);

            await context.SaveChangesAsync();
        }
    }
}