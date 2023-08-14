using AutoMapper;
using EduProject.Contexts;
using EduProject.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BlogController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Blogs =await _context.Blogs.AsNoTracking().ToListAsync();

            var blogViewModel = _mapper.Map<List<BlogViewModel>>(Blogs);


            return View(blogViewModel);
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult LeftSidebar()
        {
            return View();
        }
        public IActionResult RightSidebar()
        {
            return View();
        }
    }
}
