namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using BirthdayPresent.Infrastructure.Data;

    internal static class SessionStatusesSeeder
    {
        internal static async Task SeedAsync(ApplicationDbContext context)
        {
            var active = new SessionStatus()
            {
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var closed = new SessionStatus()
            {
                Status = "Closed",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await context.SessionStatuses.AddRangeAsync(active, closed);
            await context.SaveChangesAsync();
        }
    }
}
