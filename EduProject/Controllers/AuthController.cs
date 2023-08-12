using EduProject.Models.Identity;
using EduProject.Services.Intefaces;
using EduProject.ViewModels;
using EduProject.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace EduProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailService _mailService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;

            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
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
            if (userName is null)
            {
                userName = await _userManager.FindByNameAsync(loginViewModel.MailOrUsername);
                if (userName is null)
                {
                    ModelState.AddModelError("", "Username/Mail or password is incoreect");
                    return View();
                }
            }
            var signInResult = await _signInManager.PasswordSignInAsync(userName, loginViewModel.Password, loginViewModel.RememberMe, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username/Mail or password is incoreect");
                return View();
            }
            if (signInResult.IsLockedOut)
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
            if (returnUrl is not null)
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
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userName = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (userName is null)
            {
                ModelState.AddModelError("Email", "User not Found");
                return View();
            }
            var Token = await _userManager.GeneratePasswordResetTokenAsync(userName);

            var link = Url.Action("ResetPassword", "Auth", new { email = forgotPasswordViewModel.Email, token = Token }, HttpContext.Request.Scheme);
            string body = await GetEmailTemplate(link);
            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = forgotPasswordViewModel.Email,
                Subject = "Reset Password",
                Body = body


            };
            await _mailService.SendEMailAsync(mailRequest);
            return RedirectToAction(nameof(Login));


        }
        private async Task<string> GetEmailTemplate(string link)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Templates", "reset-password.html");
            using StreamReader streamReader = new StreamReader(path);
            string result = await streamReader.ReadToEndAsync();
            return result.Replace("[link]", link);
            

        }
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
            {
                return NotFound();
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel,string email,string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
            {
                return NotFound();
            }

           IdentityResult identityResult = await _userManager.ResetPasswordAsync(User,token,resetPasswordViewModel.Password);
            if(!identityResult.Succeeded)
            {
                foreach(var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View();

            }
            return RedirectToAction(nameof(Login));



        }


     }
}
