namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class DbSeeder
    {
        public static async Task EnsureDatabaseSeeded(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await MigrateDatabaseAsync(context);

                if (!context.Gifts.Any() && !context.Users.Any())
                {
                    await SeedDatabaseAsync(context, userManager);
                }
            }
        }

        private static async Task MigrateDatabaseAsync(ApplicationDbContext context)
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                await context.Database.MigrateAsync();
            }
        }

        private static async Task SeedDatabaseAsync(ApplicationDbContext context, UserManager<User> userManager)
        {
            await GiftSeeder.SeedDataAsync(context);
            await EmployeeSeeder.SeedDataAsync(context, userManager);
        }
    }
}
