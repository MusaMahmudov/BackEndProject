using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Areas.Admin.ViewModels.UserViewModels;
using EduProject.Contexts;
using EduProject.Exceptions;
using EduProject.Models;
using EduProject.Models.Identity;
using EduProject.Services.Implemantations;
using EduProject.Services.Intefaces;
using EduProject.Utils.Enums;
using EduProject.ViewModels;
using EduProject.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(AppDbContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper,IMailService mailService,IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {

            List<AppUser> users = await _context.Users.AsNoTracking().ToListAsync();
            List<UserViewModel> userViewModel = _mapper.Map<List<UserViewModel>>(users);
            return View(userViewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(string Id)
        {
            var User =await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == Id);
            if (User is null)
                return NotFound();
            
            var UserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(ur=>ur.UserId == User.Id);
            if (UserRole is null)
                return NotFound();
            IdentityRole role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == UserRole.RoleId);
            if (role is null)
                return NotFound();


            DetailUserViewModel detailUserViewModel = new DetailUserViewModel()
            {
                UserName = User.UserName,
                Email = User.Email,
                Fullname = User.Fullname,
                Role = role.Name,
                IsActive = User.IsActive,
                EmailConfirmed = User.EmailConfirmed,

            };
            return View(detailUserViewModel);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string Id)
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");


            var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null)
                return NotFound();

            ChangeUserViewModel changeUserViewModel = _mapper.Map<ChangeUserViewModel>(User);
            return View(changeUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(ChangeUserViewModel changeUserViewModel, string Id)
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

            if (changeUserViewModel is null) return BadRequest();

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == Id);

            if (userRole is null)
            {
                return NotFound();
            }
            _context.UserRoles.Remove(userRole);
            await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = userRole.UserId, RoleId = changeUserViewModel.RoleId });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(string Id)
        {
             
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null) 
                return NotFound();
             
            StatusUserViewModel statusUserViewModel = _mapper.Map<StatusUserViewModel>(User);
           

            return View(statusUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(StatusUserViewModel statusUserViewModel,string Id)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null)
                return NotFound();
            
            if(!User.IsActive)
            {
                User.IsActive = true;
            }
            else
            {
                User.IsActive = false;
            }


            _context.Users.Update(User);
          await  _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var newUser = _mapper.Map<AppUser>(createUserViewModel);
            if (newUser is null)
            {
                return BadRequest();
            }

            var identityResult = await _userManager.CreateAsync(newUser, createUserViewModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();


            }
           
            var Token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var Link = Url.Action("ConfirmEmail", "User", new { email = createUserViewModel.Email, token = Token }, HttpContext.Request.Scheme);
            var body = await GetEmailTemplate(Link);
            MailRequest mailRequest = new MailRequest()
            {
                Subject = "Confirm Mail",
                ToEmail = createUserViewModel.Email,
                Body = body
            };
            await _mailService.SendEMailAsync(mailRequest);

            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            return RedirectToAction(nameof(Index));




        }
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                return NotFound();
            }
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
            {
                return NotFound();
            }

            IdentityResult identityResult = await _userManager.ConfirmEmailAsync(User, token);
            if (!identityResult.Succeeded)
            {
                return BadRequest();
            }
            User.IsActive = true;

            _context.Users.Update(User);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Update(string Id)
        {
           
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User is null)
            {
                return NotFound();
            }
            var updateUserViewModel = _mapper.Map<UpdateUserViewModel>(User);


            return View(updateUserViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel, string Id)
        {
           
            var User = await _context.Users.FirstOrDefaultAsync(c => c.Id == Id);
            User.NormalizedUserName = updateUserViewModel.UserName.ToUpper();
            User.NormalizedEmail = updateUserViewModel.Email.ToUpper();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User is null)
            {
                return NotFound();
            }






            User = _mapper.Map(updateUserViewModel, User);


            _context.Users.Update(User);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}
