namespace BirthdayPresent.ConfigExtensions
{
    using BirthdayPresent.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseConfig
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
