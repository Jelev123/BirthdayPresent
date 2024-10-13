namespace BirthdayPresent.ConfigExtensions
{
    using BirthdayPresent.Core.Interfaces.Employee;
    using BirthdayPresent.Core.Interfaces.Gift;
    using BirthdayPresent.Core.Services.Employee;
    using BirthdayPresent.Core.Services.Gift;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public static class ServiceRegistrator
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpClient();
        }
    }
}
