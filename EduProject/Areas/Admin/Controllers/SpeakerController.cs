using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminEventViewModels;
using EduProject.Areas.Admin.ViewModels.AdminSpeakerViewModels;
using EduProject.Contexts;
using EduProject.Exceptions;
using EduProject.Models;
using EduProject.Services.Intefaces;
using EduProject.Utils.Enums;
using EduProject.ViewModels.SpeakerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeakerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public SpeakerController(AppDbContext context,IMapper mapper,IWebHostEnvironment webHostEnvironment,IFileService fileService)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Speakers =await _context.Speakers.OrderByDescending(s=>s.CreatedDate).AsNoTracking().ToListAsync();

            var speakerViewModel = _mapper.Map<List<SpeakerViewModel>>(Speakers);

            return View(speakerViewModel);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Create(CreateSpeakerViewModel createSpeakerViewModel)
        {

            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (createSpeakerViewModel is null)
            {
                return NotFound();
            }
            string FileName = string.Empty;
            var newSpeaker = _mapper.Map<Speaker>(createSpeakerViewModel);
            newSpeaker.IsDeleted = false;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker");
            try
            {
                FileName = await _fileService.CreateFileAsync(createSpeakerViewModel.Image, path);

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

            newSpeaker.Image = FileName;

            List<EventSpeaker> events = new List<EventSpeaker>();
            for (int i = 0; i < createSpeakerViewModel.eventId.Count(); i++)
            {
                EventSpeaker eventSpeaker = new EventSpeaker()
                {

                    EventId = createSpeakerViewModel.eventId[i],
                    SpeakerId = newSpeaker.Id,
                };

                events.Add(eventSpeaker);
            }
            newSpeaker.eventSpeakers = events;
            await _context.Speakers.AddAsync(newSpeaker);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Details(int Id)
        {
            var Speaker = await _context.Speakers.Include(e => e.eventSpeakers).ThenInclude(e => e.Event).AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
            if (Speaker is null)
            {
                return View();
            }
            var detailSpeakerViewModel = _mapper.Map<DetailSpeakerViewModel>(Speaker);
            return View(detailSpeakerViewModel);

        }
        public async Task<IActionResult> Delete(int Id)
        {
            if (_context.Speakers.Count() <= 2)
            {
                return BadRequest();
            }
            var Speaker = await _context.Speakers.FirstOrDefaultAsync(e => e.Id == Id);
            if (Speaker is null)
            {
                return NotFound();
            }
            var speakerViewModel = _mapper.Map<SpeakerViewModel>(Speaker);
            return View(speakerViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeleteEvent(int Id)
        {
            if (_context.Speakers.Count() <= 2)
            {
                return BadRequest();
            }
            var Speaker = await _context.Speakers.FirstOrDefaultAsync(e => e.Id == Id);
            if (Speaker is null)
            {
                return NotFound();
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker", Speaker.Image);
            _fileService.DeteleFile(path);
            Speaker.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");
            var Speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Speaker is null)
            {
                return NotFound();
            }
            var updateSpeakerViewModel = _mapper.Map<UpdateSpeakerViewModel>(Speaker);


            return View(updateSpeakerViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Update(UpdateSpeakerViewModel updateSpeakerViewModel, int Id)
        {
            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

            var Speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == Id);
            List<EventSpeaker> EventSpeakers = await _context.EventSpeaker.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Speaker is null)
            {
                return NotFound();
            }
            string FileName = Speaker.Image;

            if (updateSpeakerViewModel.Image is not null)
            {
                try
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker");
                    _fileService.DeteleFile(Path.Combine(path, FileName));

                    FileName = await _fileService.CreateFileAsync(updateSpeakerViewModel.Image, path);

                    Speaker.Image = FileName;
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
            }
            if (updateSpeakerViewModel.eventId is not null)
            {

                EventSpeakers.RemoveAll(Speaker => Speaker.SpeakerId == Speaker.Id);



                List<EventSpeaker> newSpeakers = new List<EventSpeaker>();

                for (int i = 0; i < updateSpeakerViewModel.eventId.Count(); i++)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker()
                    {
                        EventId = updateSpeakerViewModel.eventId[i],
                        SpeakerId =Speaker.Id
                    };


                    newSpeakers.Add(eventSpeaker);

                }
                Speaker.eventSpeakers = newSpeakers;
            }


            Speaker = _mapper.Map(updateSpeakerViewModel, Speaker);
            Speaker.Image = FileName;


            _context.Speakers.Update(Speaker);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
