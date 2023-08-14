using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminEventViewModels;
using EduProject.Contexts;
using EduProject.Exceptions;
using EduProject.Models;
using EduProject.Services.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public EventController(AppDbContext context,IMapper mapper, IWebHostEnvironment webHostEnvironment,IFileService fileService)
        {
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var Events = _context.Events.AsNoTracking().ToList();
            
            var adminEventViewModel = _mapper.Map<List<AdminEventViewModel>>(Events);
            return View(adminEventViewModel);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(),"Id","Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Create(CreateEventViewModel createEventViewModel)
        {
           
            ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (createEventViewModel is null)
            {
                return NotFound();
            }
            string FileName = string.Empty;
            var newEvent = _mapper.Map<Event>(createEventViewModel);
            newEvent.IsDeleted = false;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event");
            try
            {
                FileName = await _fileService.CreateFileAsync(createEventViewModel.Image, path);

            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();

            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }

            newEvent.Image = FileName;
            
            List<EventSpeaker> speakers = new List<EventSpeaker>();
            for(int i = 0;i  < createEventViewModel.SpeakerId.Count(); i++)
            {
                EventSpeaker eventSpeaker = new EventSpeaker()
                {
                    
                    SpeakerId = createEventViewModel.SpeakerId[i],
                    EventId = newEvent.Id,
                };

                speakers.Add(eventSpeaker);
            }
            newEvent.eventSpeakers = speakers;
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Details(int Id)
        {
            var Event = await _context.Events.Include(e=>e.eventSpeakers).ThenInclude(e=>e.Speaker).AsNoTracking().FirstOrDefaultAsync(e=>e.Id==Id);
            if(Event is null)
            {
                return View();
            }
            var DetailEventViewModel = _mapper.Map<AdminDetailEventViewModel>(Event);
            return View(DetailEventViewModel);

        }
        public async Task<IActionResult> Delete(int Id)
        {
            if (_context.Events.Count() <= 3)
            {
                return BadRequest();
            }
            var Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == Id);
            if(Event is null)
            {
                return NotFound();
            }
            var adminEventViewModel = _mapper.Map<AdminEventViewModel>(Event);
            return View(adminEventViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeleteEvent(int Id)
        {
            if (_context.Events.Count() <= 3)
            {
                return BadRequest();
            }
            var Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == Id);
            if (Event is null)
            {
                return NotFound();
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", Event.Image);
            _fileService.DeteleFile(path);
            Event.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
