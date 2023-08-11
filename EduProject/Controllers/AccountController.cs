using AutoMapper;
using EduProject.Contexts;
using EduProject.Models.Identity;
using EduProject.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;  
        public AccountController(IMapper mapper, AppDbContext context,RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserViewModel createUserViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(createUserViewModel is null)
            {
                return BadRequest();
            }
            var User =  _mapper.Map<AppUser>(createUserViewModel);
            User.IsActive = true;
            if(User is null)
            {
                return BadRequest();
            }
            var identityResult = await _userManager.CreateAsync(User,createUserViewModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach(var error in  identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                    
                }
                
            }

            await _userManager.AddToRoleAsync(User, "Member");
            return RedirectToAction("Index","Home");















        }
    }
}
