namespace BirthdayPresent.ConfigExtensions
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using BirthdayPresent.Infrastructure.Data;
    using Microsoft.AspNetCore.Identity;

    public static class IdentityConfig
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<Employee>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
               .AddRoles<IdentityRole<int>>()
               .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
