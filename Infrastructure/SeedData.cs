using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Enums;

namespace Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Users.Any()) return;

            var hasher = new PasswordHasher<User>();

            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Role = UserRole.Admin,
                    PasswordHash = hasher.HashPassword(null, "Admin@123")
                },
                new User
                {
                    Username = "staff",
                    Role = UserRole.User,
                    PasswordHash = hasher.HashPassword(null, "User@123")
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
