using Microsoft.AspNetCore.Mvc;

namespace EduProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
