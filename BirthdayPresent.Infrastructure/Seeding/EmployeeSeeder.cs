namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class EmployeeSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext data, UserManager<User> userManager)
        {

            var employee1 = new User()
            {
                FirstName = "Pesho",
                LastName =  "Petrov",
                UserName = "Pesho123",
                Email = "peter1@abv.bg",
                NormalizedEmail = "peter1@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Pesho".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,
            };
            var employee2 = new User()
            {
                FirstName = "Todor",
                LastName = "Petrov",
                UserName = "Todor123",
                Email = "todor1@abv.bg",
                NormalizedEmail = "todor1@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Todor1".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,
            };
            var employee3 = new User()
            {
                FirstName = "Marko",
                LastName = "Petrov",
                UserName = "Marko123",
                Email = "marko@abv.bg",
                NormalizedEmail = "marko@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Marko".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,

            };


            await userManager.CreateAsync(employee1, "Asd123!!");
            await userManager.CreateAsync(employee2, "Asd123!!");
            await userManager.CreateAsync(employee3, "Asd123!!");
            await data.SaveChangesAsync();
        }
    }
}


//UserName = "admin",
//                Email = "admin@yahoo.com",
//                NormalizedEmail = "admin@yahoo.com".ToUpper(),
//                EmailConfirmed = true,
//                NormalizedUserName = "admin".ToUpper(),
//                SecurityStamp = Guid.NewGuid().ToString(),
//                CreationDate = DateTime.UtcNow,
//                LastModifiedOn = DateTime.UtcNow
