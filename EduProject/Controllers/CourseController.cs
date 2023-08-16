using AutoMapper;
using EduProject.Contexts;
using EduProject.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
      
        public CourseController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;

            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();

            var courseViewModel = _mapper.Map<List<CoursePageViewModel>>(courses);


            return View(courseViewModel);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var course =await _context.Courses.Include(c=>c.courseCategories).ThenInclude(c=>c.Category).FirstOrDefaultAsync(c=>c.Id==Id);
            if (course is null)
            {
                return NotFound();
            }
            var courseViewModel = _mapper.Map<PageDetailCourseViewModel>(course);


            return View(courseViewModel);
        }
    }
}
