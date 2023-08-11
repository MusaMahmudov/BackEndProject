using AutoMapper;
using EduProject.Contexts;
using EduProject.Models;
using EduProject.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TeacherController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Teachers = _context.Teachers.AsNoTracking().ToList();
            List<TeacherViewModel> teacherViewModel = _mapper.Map<List<TeacherViewModel>>(Teachers);
            return View(teacherViewModel);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var teacher =  _context.Teachers.Include(t=>t.TeacherSkill).ThenInclude(t=>t.Skill).FirstOrDefaultAsync(t=>t.Id == Id);
            if (teacher is null)
            {
                return NotFound();
            }
            var detailTeacherViewModel = _mapper.Map<DetailTeacherViewModel>(teacher);




            return View(detailTeacherViewModel);
        }
    }
}
