using AutoMapper;
using EduProject.Contexts;
using EduProject.ViewModels.EventViewModels;
using EduProject.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    
     public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public EventController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           
            var Events =await _context.Events.AsNoTracking().ToListAsync();
            List<EventViewModel> EventsViewModel =  _mapper.Map<List<EventViewModel>>(Events) ;
            return View(EventsViewModel);

            
        }
        public async  Task<IActionResult> Details(int Id)
        {
            var Event = await _context.Events.Include(e=>e.eventSpeakers).ThenInclude(e=>e.Speaker).FirstOrDefaultAsync(e=>e.Id == Id);
            if (Event is null)
            {
                return NotFound();
            }
            var detailEventviewModel =  _mapper.Map<DetailEventViewModel>(Event) ;

            return View(detailEventviewModel);
        }
    }
}
