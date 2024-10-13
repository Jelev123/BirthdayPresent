namespace BirthdayPresent.ConfigExtensions
{
    using BirthdayPresent.Core.Interfaces.Employee;
    using BirthdayPresent.Core.Interfaces.Gift;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using BirthdayPresent.Core.Services.Employee;
    using BirthdayPresent.Core.Services.Gift;
    using BirthdayPresent.Core.Services.VoteSession;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public static class ServiceRegistrator
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<UserManager<User>>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IVoteSessionService, VoteSessionService>();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpClient();
        }
    }
}
