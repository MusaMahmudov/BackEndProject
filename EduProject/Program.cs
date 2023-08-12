using EduProject.Contexts;
using EduProject.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(c =>
{
    c.LoginPath = "/Auth/Login";
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers = false;

});
var app = builder.Build();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name:"default",
    pattern:"{Controller=Home}/{Action=Index}/{Id?}"
    );
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
   var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
   await initializer.UserSeedAsync();
   await initializer.InitializerAsync();
}

app.UseStaticFiles();
app.Run();
