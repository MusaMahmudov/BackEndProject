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
        public async Task<IActionResult> Delete(int Id)
        {
            var Slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == Id);
            if (Slider == null)
            {
                return NotFound();
            }
            var deleteSliderViewModel = _mapper.Map<DeleteSliderViewModel>(Slider);
            return View(deleteSliderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeleteSlider(int Id)
        {
            var Slider = await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == Id);
            if(Slider is null)
            {
                return NotFound();
            }
           
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "slider", Slider.Image);
             _fileService.DeteleFile(path);
            Slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Update(int Id)
        {
            var Slider = await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == Id);
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(Slider is null)
            {
                return NotFound();
            }
            var updateSliderViewModel = _mapper.Map<UpdateSliderViewModel>(Slider);


            return View(updateSliderViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateSliderViewModel updateSliderViewModel,int Id)
        {
            var Slider = await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == Id);

            if (!ModelState.IsValid)
            {
                return View();
            }
            if(Slider is null)
            {
                return NotFound();
            }
            string FileName = Slider.Image;

            if(updateSliderViewModel.Image is not null)
            {
                try
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets","img","slider");
                    _fileService.DeteleFile(Path.Combine(path, FileName));

                    FileName = await _fileService.CreateFileAsync(updateSliderViewModel.Image,path);
                   
                    Slider.Image = FileName;
                }
                catch (FileTypeException ex)
                {
                    ModelState.AddModelError("Image",ex.Message);
                    return View();
                }
                catch (FileSizeException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    return View();
                }
            }
            Slider = _mapper.Map(updateSliderViewModel, Slider);

            Slider.Image = FileName;

             _context.Sliders.Update(Slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));









        }

    }
}
