using AutoMapper;
using EduProject.Contexts;
using EduProject.Models.Identity;
using EduProject.Services.Intefaces;
using EduProject.Utils.Enums;
using EduProject.ViewModels;
using EduProject.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailService _mailService;
        public AccountController(IMapper mapper, AppDbContext context,RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager,IWebHostEnvironment webHostEnvironment,IMailService mailService)
        {
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
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
            var Token =await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var Link = Url.Action("ConfirmEmail","Account",new { email = createUserViewModel.Email ,token = Token  },HttpContext.Request.Scheme);
            var body =await GetEmailTemplate(Link);
            MailRequest mailRequest = new MailRequest()
            {
                Subject = "Confirm Mail",
                ToEmail = createUserViewModel.Email,
                Body = body
            };
           await _mailService.SendEMailAsync(mailRequest);

            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            return RedirectToAction("login","auth");
        }
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if(string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                return NotFound();
            }
            var User  = await _userManager.FindByEmailAsync(email);
            if(User is null)
            {
                return NotFound();
            }

          IdentityResult identityResult = await _userManager.ConfirmEmailAsync(User,token);
            if (!identityResult.Succeeded)
            {
               foreach(var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                    return RedirectToAction(nameof(Register));
                }
            }
            return RedirectToAction(nameof(Success));


        }
        
        public IActionResult Success()
        {
            return Ok("Success");
        }
        private async Task<string> GetEmailTemplate(string link)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Templates", "confirm-email.html");
            using StreamReader streamReader = new StreamReader(path);
            string result = await streamReader.ReadToEndAsync();
            return result.Replace("[link]", link);


        }
    }
}
