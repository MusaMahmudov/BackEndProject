using AutoMapper;
using EduProject.Contexts;
using EduProject.ViewModels;
using EduProject.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public HomeController(AppDbContext context,IMapper mapper)
        {
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
    }
}
