using AutoMapper;
using EduProject.Areas.Admin.ViewModels.AdminCourseViewModels;
using EduProject.Areas.Admin.ViewModels.AdminTeacherViewModel;
using EduProject.Contexts;
using EduProject.Exceptions;
using EduProject.Models;
using EduProject.Services.Intefaces;
using EduProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public TeacherController(AppDbContext context,IMapper mapper, IWebHostEnvironment webHostEnvironment,IFileService fileService)
        {
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var teachers = _context.Teachers.ToList();

            var teaherViewModels = _mapper.Map<List<AdminTeacherViewModel>>(teachers);
            return View(teaherViewModels);
        }
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Create(CreateTeacherViewModel createTeacherViewModel)
        {

            ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (createTeacherViewModel is null)
            {
                return NotFound();
            }
            string FileName = string.Empty;
            var newTeacher = _mapper.Map<Teacher>(createTeacherViewModel);
            newTeacher.IsDeleted = false;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher");
            try
            {
                FileName = await _fileService.CreateFileAsync(createTeacherViewModel.Image, path);

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

            newTeacher.Image = FileName;
            if(createTeacherViewModel.SkillId is not null)
            {
				List<TeacherSkill> Skills = new List<TeacherSkill>();
				for (int i = 0; i < createTeacherViewModel.SkillId.Count(); i++)
				{
					TeacherSkill teacherSkill = new TeacherSkill()
					{

						TeacherId = newTeacher.Id,
						SkillId = createTeacherViewModel.SkillId[i],
					};

					Skills.Add(teacherSkill);
				}
				newTeacher.TeacherSkill = Skills;
			}
           
            await _context.Teachers.AddAsync(newTeacher);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int Id)
        {
            var teacher = await _context.Teachers.Include(c => c.TeacherSkill).ThenInclude(c => c.Skill).Include(t=>t.socialMedia).AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
            if (teacher is null)
            {
                return BadRequest();
            }
            var teacherSkills =await _context.TeacherSkill.Where(ts=>ts.TeacherId == Id).ToListAsync();



            var detailTeacherViewModel = _mapper.Map<AdminDetailTeacherViewModel>(teacher);
            for (int i = 0; i < teacherSkills.Count(); i++)
            {
                detailTeacherViewModel.Percent[i] = teacherSkills[i].Percent;
            }
            return View(detailTeacherViewModel);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (_context.Courses.Count() <= 3)
            {
                return BadRequest();
            }
            var Course = await _context.Courses.FirstOrDefaultAsync(e => e.Id == Id);
            if (Course is null)
            {
                return NotFound();
            }
            var courseViewModel = _mapper.Map<CourseViewModel>(Course);
            return View(courseViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(int Id)
        {
            if (_context.Courses.Count() <= 3)
            {
                return BadRequest();
            }
            var Course = await _context.Courses.FirstOrDefaultAsync(e => e.Id == Id);
            if (Course is null)
            {
                return NotFound();
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", Course.Image);
            _fileService.DeteleFile(path);
            Course.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "Id", "Name");
            var Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Course is null)
            {
                return NotFound();
            }
            var updateCourseViewModel = _mapper.Map<UpdateCourseViewModel>(Course);


            return View(updateCourseViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Update(UpdateCourseViewModel updateCourseViewModel, int Id)
        {
            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "Id", "Name");

            var Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == Id);
            List<CourseCategory> courseCategories = await _context.CourseCategories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Course is null)
            {
                return NotFound();
            }
            string FileName = Course.Image;

            if (updateCourseViewModel.Image is not null)
            {
                try
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course");
                    _fileService.DeteleFile(Path.Combine(path, FileName));

                    FileName = await _fileService.CreateFileAsync(updateCourseViewModel.Image, path);

                    Course.Image = FileName;
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
            if (updateCourseViewModel.CategoryId is not null)
            {

                courseCategories.RemoveAll(Course => Course.CourseId == Course.Id);



                List<CourseCategory> newCategories = new List<CourseCategory>();

                for (int i = 0; i < updateCourseViewModel.CategoryId.Count(); i++)
                {
                    CourseCategory courseCategory = new CourseCategory()
                    {
                        CourseId = Course.Id,
                        CategoryId = updateCourseViewModel.CategoryId[i]
                    };


                    newCategories.Add(courseCategory);

                }
                Course.courseCategories = newCategories;
            }


            Course = _mapper.Map(updateCourseViewModel, Course);
            Course.Image = FileName;


            _context.Courses.Update(Course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
