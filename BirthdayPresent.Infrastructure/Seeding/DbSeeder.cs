namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbSeeder
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
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();

                await MigrateDatabaseAsync(context);

                if (!await context.Users.AnyAsync())
                {
                    await SeedDatabaseAsync(roleManager, userManager, context);
                }
            }
        }

        private static async Task MigrateDatabaseAsync(ApplicationDbContext context)
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        private static async Task SeedDatabaseAsync(RoleManager<IdentityRole<int>> roleManager, UserManager<Employee> userManager, ApplicationDbContext context)
        {
            await RolesSeeder.SeedAsync(roleManager);
            await EmployeesSeeder.SeedAsync(userManager);
            await GiftsSeeder.SeedAsync(context);
            await SessionStatusesSeeder.SeedAsync(context);
        }
    }
}
