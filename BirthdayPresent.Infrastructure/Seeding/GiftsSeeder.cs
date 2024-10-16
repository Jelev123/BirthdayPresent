namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;

    internal static class GiftsSeeder
    {
        internal static async Task SeedAsync(ApplicationDbContext data)
        {
            var gift = new List<Gift>()
            {
                new Gift()
                {
                    Name = "Head phones",
                    Description = "Model: Bose. Best quality",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Gift()
                {
                    Name = "Keyboard",
                    Description = "Model: Razer Ornata V3",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Gift()
                {
                    Name = "Mouse",
                    Description = "Model: Viper Ultimate",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },

                 new Gift()
                {
                    Name = "Mouse",
                    Description = "Model: Logitech Mx Master 3",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                  new Gift()
                {
                    Name = "Chair",
                    Description = "Model: Chair Pro",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
            };

            await data.Gifts.AddRangeAsync(gift);
            await data.SaveChangesAsync();
        }
    }
}
