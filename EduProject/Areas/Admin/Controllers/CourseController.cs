using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
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
            var courses = _context.Courses.AsNoTracking().ToList();

            var courseViewModel = _mapper.Map<List<CourseViewModel>>(courses);

            return View(courseViewModel);
        }
    }
}
