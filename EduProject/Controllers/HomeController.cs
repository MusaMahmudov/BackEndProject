using AutoMapper;
using EduProject.Contexts;
using EduProject.Models.Identity;
using EduProject.Services.Intefaces;
using EduProject.ViewModels;
using EduProject.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context,IMapper mapper,IMailService mailService,UserManager<AppUser> userManager)
        {
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Subscribe(string email)
        //{
        //    var token =await _userManager.GenerateEmailConfirmationTokenAsync();
        //    string link = Url.Action("Success","Home", {});
        //    string body = await GetEmailTemplateAsync();

            

        //    MailRequest mailRequest = new MailRequest()
        //    {
        //        Subject = "subscribe confirm",
        //        ToEmail = email,
        //        Body = body
                
        //    };

        //    await _mailService.SendEMailAsync();

        //}
        //private async Task<string> GetEmailTemplateAsync(string link)
        //{
        //    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Templates", "confirm-email.html");
        //    using StreamReader streamReader = new StreamReader(path);
        //    string result = await streamReader.ReadToEndAsync();
        //    return result.Replace("[link]", link);


        //}
    }
}
