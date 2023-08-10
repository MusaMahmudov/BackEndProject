var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


var app = builder.Build();
app.MapControllerRoute(
    name:"default",
    pattern:"{Controller=Home}/{Action=Index}/{Id?}"
    );
app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.UseStaticFiles();
app.Run();
