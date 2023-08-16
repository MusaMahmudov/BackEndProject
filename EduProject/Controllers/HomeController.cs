using AutoMapper;
using EduProject.Contexts;
using EduProject.Models.Identity;
using EduProject.Services.Intefaces;
using EduProject.ViewModels;
using EduProject.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduProject.Models;
using NuGet.Common;

namespace EduProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context,IMapper mapper,IMailService mailService,UserManager<AppUser> userManager,IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mailService = mailService;
            _mapper = mapper;
            _context = context;
        }
        public IActionResult Index()
        {
            var Event = _context.Events.AsNoTracking().ToList();
            List<EventViewModel> eventViewModel = _mapper.Map<List<EventViewModel>>(Event);
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = _context.Sliders.AsNoTracking().ToList(),
                Events = eventViewModel
            };

            return View(homeViewModel);
        }
      



        public async Task<IActionResult> Subscribe()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user =await _context.Users.FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name);
                if (user is null)
                {
                    return BadRequest();
                }
                if(await _context.subscribeUsers.FirstOrDefaultAsync(u=>u.Email == user.Email) is not null)
                {
                    
                    return RedirectToAction(nameof(Error));
                }
                    


                var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string link = Url.Action("Success", "Home", new { token = Token, Email = user.Email }, HttpContext.Request.Scheme);
                string body = await GetEmailTemplateAsync(link);

                MailRequest mailRequest = new MailRequest()
                {
                    Subject = "subscribe confirm",
                    ToEmail = user.Email,
                    Body = body

                };
                await _mailService.SendEMailAsync(mailRequest);

            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (!User.Identity.IsAuthenticated)
            {
               
                if (await _context.subscribeUsers.FirstOrDefaultAsync(u => u.Email == email) is not null)
                {
                    return RedirectToAction(nameof(Error));
                }
              

                var Token = Guid.NewGuid().ToString() ;
                string link = Url.Action("Success", "Home", new { token = Token , Email = email }, HttpContext.Request.Scheme);
                string body = await GetEmailTemplateAsync(link);

                MailRequest mailRequest = new MailRequest()
                {
                    Subject = "subscribe confirm",
                    ToEmail = email,
                    Body = body

                };
                await _mailService.SendEMailAsync(mailRequest);

            }


            return RedirectToAction("Index");
        }
        private async Task<string> GetEmailTemplateAsync(string link)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Templates", "confirm-email.html");
            using StreamReader streamReader = new StreamReader(path);
            string result = await streamReader.ReadToEndAsync();
            return result.Replace("[link]", link);


        }
        public async Task<IActionResult> Success(string token,string email)
        {
            if(string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email)) 
            {
                return BadRequest();
            }
            if(await _context.subscribeUsers.FirstOrDefaultAsync(s=>s.Email == email) is not null)
            {
                return BadRequest();
            }

            SubscribeUsers subscribeUser = new SubscribeUsers()
            {
                Email = email,
            };
            await _context.subscribeUsers.AddAsync(subscribeUser);
           await _context.SaveChangesAsync();

            return Ok("Success");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
