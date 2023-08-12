using EduProject.Models.Identity;
using EduProject.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userName = await _userManager.FindByEmailAsync(loginViewModel.MailOrUsername);
            if(userName is null) 
            {
                userName = await _userManager.FindByNameAsync(loginViewModel.MailOrUsername);
                if(userName is null)
                {
                    ModelState.AddModelError("", "Username/Mail or password is incoreect");
                    return View();
                }
            }
          var signInResult = await _signInManager.PasswordSignInAsync(userName, loginViewModel.Password,false,true);
            if(!signInResult.Succeeded) 
            {
                ModelState.AddModelError("", "Username/Mail or password is incoreect");
                return View();
            }
            if(signInResult.IsLockedOut) 
            {
                ModelState.AddModelError("", "The number of unsuccessful attempts exceeded the limit, try again after 1 minute");
                return View();
            }

            if (!userName.LockoutEnabled)
            {
                userName.LockoutEnabled = true;
                userName.LockoutEnd = null;
                await _userManager.UpdateAsync(userName);
            }
            if(returnUrl is not null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }   
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));

        }
    }
}
