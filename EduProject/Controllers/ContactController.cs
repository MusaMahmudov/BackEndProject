using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
