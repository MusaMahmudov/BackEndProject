using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
