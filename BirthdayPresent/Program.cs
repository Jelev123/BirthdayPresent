using BirthdayPresent.ConfigExtensions;
using BirthdayPresent.Infrastructure.Seeding;
using BirthdayPresent.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

DbSeeder.EnsureDatabaseSeeded(app.Services).GetAwaiter().GetResult();

app.UseMiddleware<ErrorHandler>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
           name: "Area",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
           name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
