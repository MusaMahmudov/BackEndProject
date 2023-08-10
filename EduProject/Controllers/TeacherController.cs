using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
