using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminSliderViewModels;
using EduProject.Contexts;
using EduProject.Exceptions;
using EduProject.Models;
using EduProject.Services.Intefaces;
using EduProject.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public SliderController(AppDbContext context,IMapper mapper, IWebHostEnvironment webHostEnvironment,IFileService fileService)
        {
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var Sliders = await _context.Sliders.AsNoTracking().ToListAsync();
            List<SliderViewModel> adminSliderViewModel = _mapper.Map<List<SliderViewModel>>(Sliders);
            return View(adminSliderViewModel);
        }
        public IActionResult Detail(int Id)
        {
            var Slider = _context.Sliders.FirstOrDefault(x => x.Id == Id);
            if (Slider == null)
            {
                return NotFound();
            }
            var detailSliderViewModel = _mapper.Map<DetailSliderViewModel>(Slider);

            return View(detailSliderViewModel);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSliderViewModel createSliderViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(createSliderViewModel is null)
            {
                return NotFound();
            }
            string FileName = string.Empty;
            var newSlider = _mapper.Map<Slider>(createSliderViewModel);
            newSlider.IsDeleted = false;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "slider");
            try
            {
                 FileName = await _fileService.CreateFileAsync(createSliderViewModel.Image, path);

            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();

            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError("Image",ex.Message);
                return View();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }

            newSlider.Image = FileName;


            await _context.Sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }

    }
}
