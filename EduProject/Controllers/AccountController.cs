using AutoMapper;
using EduProject.Contexts;
using EduProject.Models.Identity;
using EduProject.Utils.Enums;
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
            if(User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserViewModel createUserViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(createUserViewModel is null)
            {
                return BadRequest();
            }
            var newUser =  _mapper.Map<AppUser>(createUserViewModel);
            newUser.IsActive = true;
            if(newUser is null)
            {
                return BadRequest();
            }
            var identityResult = await _userManager.CreateAsync(newUser, createUserViewModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach(var error in  identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    
                }
                return View();


            }

            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            return RedirectToAction("Index","Home");
        }
    }
}
