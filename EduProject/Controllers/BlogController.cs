using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
