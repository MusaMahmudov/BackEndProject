using EduProject.Models.Identity;
using EduProject.Utils.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Contexts;

public class AppDbContextInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    public AppDbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,AppDbContext context)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitializerAsync()
    {
        await _context.Database.MigrateAsync();
    }
    public async Task UserSeedAsync()
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString()});
        }
        AppUser adminUser = new AppUser()
        {
            EmailConfirmed = true,
            UserName = "admin",
            Fullname = "adminAdminov",
            IsActive = true,
            Email = "musafm@code.edu.az"
        };
        await _userManager.CreateAsync(adminUser,"Salam123!");

        await _userManager.AddToRoleAsync(adminUser,Roles.Admin.ToString());


    }


}
