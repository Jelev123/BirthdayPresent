namespace BirthdayPresent.Infrastructure.Seeding
{
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;

    internal static class EmployeesSeeder
    {
        internal static async Task SeedAsync(UserManager<Employee> userManager)
        {
            var employee1 = new Employee()
            {
                FirstName = "Pesho",
                LastName =  "Petrov",
                UserName = "Pesho",
                Email = "peter1@abv.bg",
                NormalizedEmail = "peter1@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Pesho".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,
            };
            var employee2 = new Employee()
            {
                FirstName = "Todor",
                LastName = "Petrov",
                UserName = "Todor",
                Email = "todor1@abv.bg",
                NormalizedEmail = "todor1@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Todor1".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,
            };
            var employee3 = new Employee()
            {
                FirstName = "Marko",
                LastName = "Petrov",
                UserName = "Marko",
                Email = "marko@abv.bg",
                NormalizedEmail = "marko@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Marko".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,

            }; var employee4 = new Employee()
            {
                FirstName = "Misho",
                LastName = "Todorov",
                UserName = "Misho",
                Email = "misho@abv.bg",
                NormalizedEmail = "misho@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Misho".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,

            }; var employee5 = new Employee()
            {
                FirstName = "Svetlio",
                LastName = "Ivanov",
                UserName = "Svetlio",
                Email = "svetlio@abv.bg",
                NormalizedEmail = "svetlio@abv.bg".ToUpper(),
                EmailConfirmed = true,
                NormalizedUserName = "Svetlio".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow,

            };


            await userManager.CreateAsync(employee1, "Asd123!!");
            await userManager.AddToRoleAsync(employee1, "regular");

            await userManager.CreateAsync(employee2, "Asd123!!");
            await userManager.AddToRoleAsync(employee1, "regular");

            await userManager.CreateAsync(employee3, "Asd123!!");
            await userManager.AddToRoleAsync(employee1, "regular");
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
