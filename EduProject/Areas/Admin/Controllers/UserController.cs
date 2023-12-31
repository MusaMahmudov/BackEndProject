﻿using AutoMapper;
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
            
            List<IdentityUserRole<string>> UserRole =  _context.UserRoles.AsNoTracking().Where(ur=>ur.UserId == User.Id).ToList();
            if (UserRole is null)
                return NotFound();

            List<IdentityRole> role = new List<IdentityRole>();
            for(int i = 0; i < UserRole.Count(); i++)
            {
                 role.AddRange(_context.Roles.Where(r => r.Id == UserRole[i].RoleId).ToList());
            }




            if (role is null)
                return NotFound();


            DetailUserViewModel detailUserViewModel = _mapper.Map<DetailUserViewModel>(User);
            detailUserViewModel.Role = role;
         
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

            var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
            if(User is null)
            {
                return NotFound();
            }
            if (changeUserViewModel is null) 
                return BadRequest();

            var userRole =  _context.UserRoles.Where(ur => ur.UserId == Id).ToList();
            
            if (userRole is null)
            {
                return NotFound();
            }

            if(changeUserViewModel.RoleId is not null)
            {
                _context.UserRoles.RemoveRange(userRole);

                List<IdentityUserRole<string>> identityUserRole = new List<IdentityUserRole<string>>();

                for (int i = 0; i < changeUserViewModel.RoleId.Count(); i++)
                {
                    IdentityUserRole<string> identityUR = new IdentityUserRole<string>();
                    identityUR.RoleId = changeUserViewModel.RoleId[i];
                    identityUR.UserId = userRole[0].UserId;
                    identityUserRole.Add(identityUR);
                }

                await _context.UserRoles.AddRangeAsync(identityUserRole);
                await _context.SaveChangesAsync();

            }







            //await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = userRole.UserId, RoleId = changeUserViewModel.RoleId });


            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(string Id)
        {
             
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null || User.Identity.Name == user.UserName || User.Identity.Name== user.NormalizedUserName) 
                return NotFound();
             
            StatusUserViewModel statusUserViewModel = _mapper.Map<StatusUserViewModel>(user);
           

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

            if ((_context.Users.Where(u => u.UserName == updateUserViewModel.UserName).Where(u => u.UserName != User.UserName).Count() == 1) || _context.Users.Where(u => u.Email == updateUserViewModel.Email).Where(u => u.Email != User.Email).Count()== 1)
            {
                ModelState.AddModelError("", "userName or Email already exists");
                return View();
            }


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
