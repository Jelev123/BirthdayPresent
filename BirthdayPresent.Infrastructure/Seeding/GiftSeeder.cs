﻿namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;

    public class GiftSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext data)
        {
            var gift = new List<Gift>()
            {
                new Gift()
                {
                    Name = "Head phones",
                    Description = "Best quality",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Gift()
                {
                    Name = "Keyboard",
                    Description = "Best quality",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Gift()
                {
                    Name = "Mouse",
                    Description = "Best quality",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
            };

            await data.Gifts.AddRangeAsync(gift);
            await data.SaveChangesAsync();
        }
    }
}
