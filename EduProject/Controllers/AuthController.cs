using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
