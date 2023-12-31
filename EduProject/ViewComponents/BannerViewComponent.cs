﻿using EduProject.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public BannerViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return View(settings);
        }
    }
}
