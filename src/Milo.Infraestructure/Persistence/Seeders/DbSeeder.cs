using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milo.Domain.Entities;
using Milo.Domain.Entities.Enums;

namespace Milo.Infraestructure.Persistence.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var context = services.GetRequiredService<MiloDbContext>();

        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var hasher = new PasswordHasher<User>();

        // Admin
        var adminEmail = configuration["Admin:Email"]!;
        var adminPassword = configuration["Admin:Password"]!;
        var adminTemp = User.Create("Admin", "Prueba", adminEmail, string.Empty, UserRole.Admin);
        var admin = User.Create("Admin", "Prueba", adminEmail,
            hasher.HashPassword(adminTemp, adminPassword), UserRole.Admin);
        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}