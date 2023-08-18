using AutoMapper;
using EduProject.Areas.Admin.ViewModels.SettingsViewModels;

using EduProject.Contexts;
using EduProject.Models;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SettingController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Index()
        {
            var settings = _context.Settings.IgnoreQueryFilters().ToList();
            return View(settings);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int Id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s=>s.Id == Id);
            if (setting is null) 
            {
              return NotFound();
            }
            var updateSettingViewModel = _mapper.Map<UpdateSettingViewModel>(setting);

            return View(updateSettingViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(UpdateSettingViewModel updateSettingViewModel,int Id)
        {
            if (updateSettingViewModel is null) 
            {
                return View();
            }
            var checkUpdate = await _context.Settings.Where(s => s.Key == updateSettingViewModel.Key).ToListAsync();
            if (checkUpdate.Count() >=1) 
            {
                ModelState.AddModelError("Key","Key already exists");
                return View();
            }
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == Id);

            if (setting is null)
            {
                return NotFound();
            }
           setting =_mapper.Map(updateSettingViewModel, setting);  
           
             _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(CreateSettingViewModel createSettingViewModel) 
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var checkUpdate = await _context.Settings.Where(s => s.Key == createSettingViewModel.Key).ToListAsync();
            if (checkUpdate.Count() >= 1)
            {
                ModelState.AddModelError("Key", "Key already exists");
                return View();
            }

            var newSetting = _mapper.Map<Setting>(createSettingViewModel);

            await _context.Settings.AddAsync(newSetting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s=>s.Id == Id);
            if (setting is null)
            {
                return NotFound();
            }
            var deleteSettingViewModel = _mapper.Map<DeleteSettingViewModel>(setting);

            return View(deleteSettingViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(DeleteSettingViewModel deleteSettingViewModel,int Id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == Id);
            if (setting is null)
            {
                return NotFound();
            }
            setting.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
